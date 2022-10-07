using api.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace api.Controllers
{
    [Authorize]
    public class CategoriaController : ApiController
    {
        private dbEntities db = new dbEntities();

        /// <summary>
        /// Recupera todas as categorias do banco de dados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<tbcategorias> GetCategorias()
        {
            return db.tbcategorias.AsNoTracking();
        }

        /// <summary>
        /// Recupera a informação de uma determinada categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCategoria(int id)
        {
            try
            {
                var oCategoria = db.tbcategorias.AsNoTracking().Where(r=> r.CategoriaID == id);
                if (oCategoria == null) return NotFound();

                return Ok(oCategoria);
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
        /// Salva uma categoria no banco de dados
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PostCategoria()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var httpRequest = HttpContext.Current.Request;

                var oCategoria = new tbcategorias()
                {
                    Nome = httpRequest.Form["Nome"]
                };

                if (!string.IsNullOrEmpty(oCategoria.Nome))
                {
                    db.Entry(oCategoria).State = EntityState.Added;
                    if (db.SaveChanges() > 0)
                    {
                        return Ok(oCategoria);
                    }
                    else return BadRequest();
                }
                else return BadRequest("Preencha corretament os campos marcados como obrigatório!");
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
        /// Atualiza os dados de uma determinada categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult PutCategoria(int id)
        {
            try
            {
                if (CategoriaExists(id))
                {
                    if (!ModelState.IsValid) return BadRequest(ModelState);
                    var httpRequest = HttpContext.Current.Request;

                    var oCategoria = db.tbcategorias.Where(r => r.CategoriaID == id).FirstOrDefault();
                    if (oCategoria != null)
                    {
                        oCategoria.Nome = httpRequest.Form["Nome"];

                        if (!string.IsNullOrEmpty(oCategoria.Nome))
                        {
                            db.Entry(oCategoria).State = EntityState.Modified;
                            if (db.SaveChanges() > 0)
                            {
                                return Ok(oCategoria);
                            }
                            else return BadRequest();
                        }
                        else return BadRequest("Preencha corretament os campos marcados como obrigatório!");
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

        /// <summary>
        /// Deleta uma determinada categoria do banco de dados
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteCategoria(int id)
        {
            try
            {
                if (CategoriaExists(id))
                {
                    var oCategoria = db.tbcategorias.Where(r => r.CategoriaID == id).FirstOrDefault();
                    if (oCategoria != null)
                    {
                        db.Entry(oCategoria).State = EntityState.Deleted;
                        if (db.SaveChanges() > 0)
                        {
                            return Ok("Categoria deletada com sucesso!");
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoriaExists(long id)
        {
            return db.tbcategorias.Count(r => r.CategoriaID == id) > 0;
        }
    }
}
