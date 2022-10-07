using System.Web;
using System.Web.Http;

namespace api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_OnBeginRequest()
        {
            var res = HttpContext.Current.Response;
            var req = HttpContext.Current.Request;
            //res.AppendHeader("Access-Control-Allow-Origin", req.Headers["Origin"]);
            //res.AppendHeader("Access-Control-Allow-Credentials", "true");
            res.AppendHeader("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
            res.AppendHeader("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");

            // ==== Respond to the OPTIONS verb =====
            if (req.HttpMethod == "OPTIONS")
            {
                res.StatusCode = 200;
                res.End();
            }
        }
    }
}
