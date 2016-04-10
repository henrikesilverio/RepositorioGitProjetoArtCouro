using System;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Microsoft.Owin;

namespace ProjetoArtCouro.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Login/Login"),
                ExpireTimeSpan = TimeSpan.FromHours(1)
            });
        }
    }
}