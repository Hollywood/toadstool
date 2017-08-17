using Toadstool.App_Start;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Web.Http;

namespace Toadstool
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(WebHookConfig.Register);
        }
    }
}
