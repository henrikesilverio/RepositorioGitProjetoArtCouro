﻿using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace ProjetoArtCouro.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                return;
            }
            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
            if (ticket == null)
            {
                return;
            }
            var formsIdentity = new FormsIdentity(ticket);
            var claimsIdentity = new ClaimsIdentity(formsIdentity);

            //foreach (var role in user.Roles)
            //{
            claimsIdentity.AddClaim(
                new Claim(ClaimTypes.Role, "Admin"));
            //}
            var principal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.Current.User = principal;
        }
    }
}
