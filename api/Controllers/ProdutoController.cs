using api.Models;
using api.UsefulClasses;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace api.Controllers
{
    [Authorize]
    public class ProdutoController : ApiController
    {
        private dbEntities db = new dbEntities();

        /// <summary>
        /// Recupera todos os produtos do banco de dados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ListaProdutos()
        {
            var listProdutos = (from p in db.tbprodutos orderby p.ProdutoID descending select new { p, fotos = db.tbprodutofotos.Where(f => f.ProdutoID == p.ProdutoID) }).ToList();
            return Ok(listProdutos);
        }

        /// <summary>
        /// Recupera as informações de im determinado produto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult SelecionaProduto(long id)
        {
            try
            {
                var oProduto = (from p in db.tbprodutos where p.ProdutoID == id select new { p, fotos = db.tbprodutofotos.Where(f => f.ProdutoID == p.ProdutoID) }).FirstOrDefault();
                if (oProduto == null)
                {
                    return NotFound();
                }

                return Ok(oProduto);
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

        /// <summary>
        /// Adiciona um produto no banco de dados
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SalvaProduto()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var httpRequest = HttpContext.Current.Request;

                var oProduto = new tbprodutos()
                {
                    Titulo = httpRequest.Form["Titulo"],
                    Descricao = httpRequest.Form["Descricao"],
                    Status = Int64.Parse(httpRequest.Form["Status"]),
                    Categorias = httpRequest.Form["Categorias"],
                    Tags = httpRequest.Form["Tags"],
                    Preco = decimal.Parse(httpRequest.Form["Preco"]),
                    PrecoPromocional = decimal.Parse(httpRequest.Form["PrecoPromocional"])
                };

                if (!string.IsNullOrEmpty(oProduto.Titulo) && !string.IsNullOrEmpty(oProduto.Descricao) && oProduto.Status > 0 && oProduto.Preco > 0)
                {
                    db.tbprodutos.Add(oProduto);
                    if (db.SaveChanges() > 0)
                    {
                        if (httpRequest.Files.Count > 0) SystemOperations.UploadFile(httpRequest.Files, "/media/produtos/", oProduto.ProdutoID);

                        return Ok(oProduto);
                    }
                    else return BadRequest();
                }
                else return BadRequest("Preencha corretamente todos os campos marcados como obrigatório!");
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

        /// <summary>
        /// Usado para alterar as informações de um determinado produto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult AtualizaProduto(int id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var httpRequest = HttpContext.Current.Request;

                var oProduto = db.tbprodutos.Where(p => p.ProdutoID == id).FirstOrDefault();
                if (oProduto != null)
                {
                    oProduto.Titulo = httpRequest.Form["Titulo"];
                    oProduto.Descricao = httpRequest.Form["Descricao"];
                    oProduto.Status = Int64.Parse(httpRequest.Form["Status"]);
                    oProduto.Categorias = httpRequest.Form["Categorias"];
                    oProduto.Tags = httpRequest.Form["Tags"];
                    oProduto.Preco = decimal.Parse(httpRequest.Form["Preco"]);
                    oProduto.PrecoPromocional = decimal.Parse(httpRequest.Form["PrecoPromocional"]);

                    if (!string.IsNullOrEmpty(oProduto.Titulo) && !string.IsNullOrEmpty(oProduto.Descricao) && oProduto.Status > 0 && oProduto.Preco > 0)
                    {
                        db.Entry(oProduto).State = EntityState.Modified;
                        db.tbprodutos.Add(oProduto);
                        if (db.SaveChanges() > 0)
                        {
                            if (httpRequest.Files.Count > 0) SystemOperations.UploadFile(httpRequest.Files, "/media/produtos/", oProduto.ProdutoID);

                            return Ok(oProduto);
                        }
                        else return BadRequest();
                    }
                    else return BadRequest("Preencha corretamente todos os campos marcados como obrigatório!");
                }
                else return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deleta um determiando produto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete()]
        public IHttpActionResult DeletaProduto(int id)
        {
            try
            {
                if (ProdutoExists(id))
                {
                    var oProduto = db.tbprodutos.Where(r => r.ProdutoID == id).FirstOrDefault();
                    if (oProduto != null)
                    {
                        db.Entry(oProduto).State = EntityState.Deleted;
                        if (db.SaveChanges() > 0)
                        {
                            return Ok("Produto deletado com sucesso!");
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

        private bool ProdutoExists(long id)
        {
            return db.tbprodutos.AsNoTracking().Count(e => e.ProdutoID == id) > 0;
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
