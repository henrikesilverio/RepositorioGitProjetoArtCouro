using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ProjetoArtCouro.Model.Models.Usuario;
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

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
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
            var response = ServiceRequest.Get<List<PermissaoModel>>(null, "api/Usuario/ObterPermissoesUsuarioLogado");
            if (response.Data == null)
            {
                return;
            }
            foreach (var permissao in response.Data.ObjetoRetorno)
            {
                claimsIdentity.AddClaim(
                    new Claim(ClaimTypes.Role, permissao.AcaoNome));
            }
            var principal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.Current.User = principal;
        }
    }
}
