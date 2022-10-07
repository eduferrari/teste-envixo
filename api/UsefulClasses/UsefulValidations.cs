using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace api.UsefulClasses
{
    public class UsefulValidations
    {
        public static string GetMD5Hash(string sData)
        {
            try
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] inputBytes = Encoding.ASCII.GetBytes(sData);
                    byte[] hash = md5.ComputeHash(inputBytes);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hash.Length; i++)
                        sb.Append(hash[i].ToString("X2"));

                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string MakeUniqueFilename(string dir, string filename)
        {
            try
            {
                string ret = filename;
                int i = 0;
                while (File.Exists(Path.Combine(dir, ret)))
                {
                    i++;
                    ret = Path.GetFileNameWithoutExtension(filename) + "-copy-" + i.ToString() + Path.GetExtension(filename);
                }
                return ret.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}