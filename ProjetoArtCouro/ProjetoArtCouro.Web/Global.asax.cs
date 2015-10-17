using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Newtonsoft.Json;
using ProjetoArtCouro.Web.Infra.Service;

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
            var tokenModel = JsonConvert.DeserializeObject<TokenModel>(ticket.UserData);
            var roles = JsonConvert.DeserializeObject<string[]>(tokenModel.roles);

            foreach (var role in roles)
            {
                claimsIdentity.AddClaim(
                    new Claim(ClaimTypes.Role, role));
            }
            var principal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.Current.User = principal;
        }
    }
}
