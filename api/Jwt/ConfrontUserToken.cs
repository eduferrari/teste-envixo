using System;
using System.Linq;
using api.Models;
using Newtonsoft.Json.Linq;

namespace api.Jwt
{
    public class ConfrontUserToken
    {
        public static bool ValidateInfo(string json, string token)
        {
            try
            {
                using (dbEntities db = new dbEntities())
                {
                    dynamic oUser = JObject.Parse(json);
                    if (oUser != null)
                    {
                        Int64 userId = Convert.ToInt64(oUser.Id);

                        if (db.tbtokens.Count(r => r.UsuarioID == userId && r.Token == token) > 0)
                        {
                            var oUsuario = db.tbusuarios.FirstOrDefault(r => r.UsuarioID == userId);
                            if (oUsuario != null)
                            {
                                DateTime expires = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                                if (oUsuario.Email == Convert.ToString(oUser.Email))
                                {
                                    if (oUsuario.Senha == Convert.ToString(oUser.Senha))
                                    {
                                        if (oUser.Validade >= expires)
                                        {
                                            return true;
                                        }
                                        else throw new Exception("O token expirou!");
                                    }
                                    else throw new Exception("A senha é inválida!");
                                }
                                else throw new Exception("O e-mail é inválido!");
                            }
                            else throw new Exception("Usuário não encontrado!");
                        }
                        else throw new Exception("Token inválido!");
                    }
                    else throw new Exception("Token inválido!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}