using System;
using System.IO;
using System.Web;
using api.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Entity;
using System.Data;

namespace api.UsefulClasses
{
    public class SystemOperations
    {

        public static bool UploadFile(HttpFileCollection sendFile, string path, long produtoId)
        {
            try
            {

                using (dbEntities db = new dbEntities())
                {


                    string savedFile = "";

                    if (sendFile.Count > 0)
                    {
                        //Valida se foi informado local para armazenas os arquivos e cria a pasta caso não exista
                        if (string.IsNullOrEmpty(path)) throw new Exception("Para prosseguir será necessário informar o local onde será salvo o arquivo!");
                        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~" + path))) Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~" + path));

                        foreach (string file in sendFile)
                        {
                            var postedFile = sendFile[file];
                            if (postedFile != null && postedFile.ContentLength > 0)
                            {
                                //Valida tamanha do arquivo: Padrão = 1 MB
                                int MaxContentLength = 1024 * 1024 * 10;
                                if (postedFile.ContentLength > MaxContentLength) throw new Exception("Por favor, envie um arquivo até 1 mb.");

                                //Valida extensões de arquivos: Padrão = .jpg, .gif, .png
                                var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                                IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                                if (!AllowedFileExtensions.Contains(ext.ToLower())) throw new Exception("Por favor, envie uma imagem do tipo .jpg,.gif,.png.");

                                //Caso passa nas validações salva o arquivo na pasta informada
                                savedFile = path + UsefulValidations.MakeUniqueFilename(HttpContext.Current.Server.MapPath("~" + path), postedFile.FileName);
                                postedFile.SaveAs(HttpContext.Current.Server.MapPath("~" + savedFile));

                                if (produtoId > 0)
                                {
                                    var oProdutoFoto = new tbprodutofotos()
                                    {
                                        Nome = savedFile,
                                        ProdutoID = produtoId
                                    };

                                    db.Entry(oProdutoFoto).State = EntityState.Added;
                                }
                            }
                        }
                    }

                    return db.SaveChanges() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }
    }
}