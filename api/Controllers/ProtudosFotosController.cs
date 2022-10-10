using api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class ProtudosFotosController : ApiController
    {
        private dbEntities db = new dbEntities();

        /// <summary>
        /// Deleta uma determiada foto cadastrada no produto
        /// </summary>
        /// <param name="produtoId"></param>
        /// <param name="fotoId"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeletaFotoProduto(int produtoId, int fotoId)
        {
            try
            {
                if (ProdutoExists(produtoId))
                {
                    var oFoto = db.tbprodutofotos.Where(r => r.ProdutoID == produtoId && r.FotoID == fotoId).FirstOrDefault();
                    if (oFoto != null)
                    {
                        db.Entry(oFoto).State = EntityState.Deleted;
                        if (db.SaveChanges() > 0)
                        {


                            return Ok("Foto vinculada ao produto(id: " + produtoId + ") deletada com sucesso!");
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
