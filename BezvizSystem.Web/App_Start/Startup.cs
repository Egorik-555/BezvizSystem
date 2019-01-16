
using System;
using System.Threading.Tasks;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Services;
using BezvizSystem.DAL;
using BezvizSystem.DAL.EF;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using BezvizSystem.BLL.DTO.Dictionary;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using System.IO;
using System.Text.RegularExpressions;

[assembly: OwinStartup(typeof(BezvizSystem.Web.App_Start.Startup))]

namespace BezvizSystem.Web.App_Start
{
    public class Startup
    {
        IServiceCreator serviceCreator = new ServiceCreator();
        string CONNECTION = "BezvizContext";

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<BezvizContext>(CreateContext);
            app.CreatePerOwinContext<BezvizUserManager>(BezvizUserManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            app.Use(typeof(MyMiddlewareClass));

        }

        private BezvizContext CreateContext()
        {
            return serviceCreator.CreateContext(CONNECTION);
        }

    }



    public class MyMiddlewareClass : OwinMiddleware
    {
        DateTime DATE = new DateTime(2019, 01, 30);

        private DateTime GetNistTime()
        {
            DateTime dateTime = DateTime.MaxValue;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://nist.time.gov/actualtime.cgi?lzbc=siqm9b");
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore); //No caching
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader stream = new StreamReader(response.GetResponseStream());
                    string html = stream.ReadToEnd();//<timestamp time=\"1395772696469995\" delay=\"1395772696469995\"/>
                    string time = Regex.Match(html, @"(?<=\btime="")[^""]*").Value;
                    double milliseconds = Convert.ToInt64(time) / 1000.0;
                    dateTime = new DateTime(1970, 1, 1).AddMilliseconds(milliseconds).ToLocalTime();
                }
            }
            catch
            {
                
            }

            return dateTime;
        }

        public MyMiddlewareClass(OwinMiddleware next)
            : base(next)
        {

        }

        public async override Task Invoke(IOwinContext context)
        {
            var nist = GetNistTime();

            if (nist > DATE)
                await context.Response.WriteAsync($"The term of the application expired ({DATE.ToShortDateString()})  :((");
            else await Next.Invoke(context);
        }
    }

}
