using api.Models;
using api.UsefulClasses;
using Newtonsoft.Json.Linq;
using System;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace api.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : ApiController
    {
        private dbEntities db = new dbEntities();

        /// <summary>
        /// Cria um token de autenticação para ser usado nas requisições da API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetToken()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var httpRequest = HttpContext.Current.Request;
                string user = httpRequest.Form["email"];
                string pass = !string.IsNullOrEmpty(httpRequest.Form["senha"]) ? UsefulValidations.GetMD5Hash(httpRequest.Form["senha"]) : string.Empty;

                if (!string.IsNullOrEmpty(pass))
                {
                    var oUsuario = db.tbusuarios.FirstOrDefault(r => r.Email == user && r.Senha == pass);
                    if (oUsuario != null)
                    {
                        string token = createToken(oUsuario);
                        if (!string.IsNullOrEmpty(token))
                        {
                            var oToken = db.tbtokens.FirstOrDefault(r => r.UsuarioID == oUsuario.UsuarioID);
                            if (oToken != null)
                            {
                                oToken.Token = token;
                                oToken.Validade = DateTime.UtcNow.AddDays(7);

                                db.Entry(oToken).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                oToken = new tbtokens()
                                {
                                    UsuarioID = oUsuario.UsuarioID,
                                    Token = token,
                                    Validade = DateTime.UtcNow.AddDays(7)
                                };
                                db.Entry(oToken).State = EntityState.Added;
                                db.SaveChanges();
                            }

                            return Ok(new
                            {
                                Id = oUsuario.UsuarioID,
                                Token = token
                            });
                        }
                        else return BadRequest();
                    }
                    else return NotFound();
                }
                else return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                Dispose(true);
            }
        }

        private string createToken(tbusuarios oUsuario)
        {
            try
            {
                //Set issued at date
                DateTime issuedAt = DateTime.UtcNow;
                //set the time when it expires
                DateTime expires = DateTime.UtcNow.AddDays(1);

                //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
                var tokenHandler = new JwtSecurityTokenHandler();
                string uri = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority;

                dynamic userInfo = new JObject();
                userInfo.Id = oUsuario.UsuarioID;
                userInfo.Email = oUsuario.Email;
                userInfo.Senha = oUsuario.Senha;
                userInfo.Modulos = new JArray("Produto", "Categoria");
                userInfo.Validade = new DateTime(expires.Year, expires.Month, expires.Day);

                //create a identity and add claims to the user which we want to log in
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim("UserInfo", Convert.ToString(userInfo)) });

                const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
                var now = DateTime.UtcNow;
                var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
                var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

                //create the jwt
                var token = (JwtSecurityToken)tokenHandler.CreateJwtSecurityToken(issuer: uri, audience: uri, subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
                var tokenString = tokenHandler.WriteToken(token);

                return tokenString;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
