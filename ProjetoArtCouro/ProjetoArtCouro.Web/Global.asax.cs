using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Newtonsoft.Json;

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
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                HttpContext.Current.Request.Cookies.Remove(".ASPXTOKEN");
                HttpContext.Current.Request.Cookies.Remove(".ASPXROLES");
                return;
            }
            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
            if (ticket == null)
            {
                return;
            }
            var formsIdentity = new FormsIdentity(ticket);
            var claimsIdentity = new ClaimsIdentity(formsIdentity);
            var rolesCookie = HttpContext.Current.Request.Cookies[".ASPXROLES"];
            if (rolesCookie != null)
            {
                var ticketRoles = FormsAuthentication.Decrypt(rolesCookie.Value);
                if (ticketRoles != null)
                {
                    var roles = JsonConvert.DeserializeObject<string[]>(ticketRoles.UserData);
                    foreach (var role in roles)
                    {
                        claimsIdentity.AddClaim(
                            new Claim(ClaimTypes.Role, role));
                    }
                }
            }
            var principal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.Current.User = principal;
        }
    }
}
