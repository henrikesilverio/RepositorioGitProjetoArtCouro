using System.Web;
using System.Web.Security;
using ProjetoArtCouro.Web.Infra.Authorization;

namespace ProjetoArtCouro.Web.Infra.Extensions
{
    public static class HttpResponseBaseExtensions
    {
        public static int SetAuthCookie(this HttpResponseBase responseBase, string userName, string token, string roles, bool rememberMe)
        {
            var cookieAuth = FormsAuthentication.GetAuthCookie(userName, rememberMe);
            var ticketAuth = FormsAuthentication.Decrypt(cookieAuth.Value);
            if (ticketAuth == null)
            {
                return 0;
            }
            var newTicketAuth = new FormsAuthenticationTicket(ticketAuth.Version, userName, ticketAuth.IssueDate, ticketAuth.Expiration,
                ticketAuth.IsPersistent, string.Empty, ticketAuth.CookiePath);
            var encTicketAuth = FormsAuthentication.Encrypt(newTicketAuth);
            cookieAuth.Value = encTicketAuth;
            cookieAuth.Expires = ticketAuth.Expiration;
            
            var cookieRoles = new HttpCookie(".ASPXROLES");
            var ticketRoles = new FormsAuthenticationTicket(ticketAuth.Version, userName, ticketAuth.IssueDate, ticketAuth.Expiration,
                ticketAuth.IsPersistent, roles, "/ROLES");
            var encTickeRoles = FormsAuthentication.Encrypt(ticketRoles);
            cookieRoles.Value = encTickeRoles;
            cookieRoles.Expires = ticketAuth.Expiration;

            var cookieToken = new HttpCookie(".ASPXTOKEN")
            {
                Value = EncryptionMD5.Encrypt(token),
                Expires = ticketAuth.Expiration,
                Path = "/"
            };

            responseBase.Cookies.Add(cookieAuth);
            responseBase.Cookies.Add(cookieToken);
            responseBase.Cookies.Add(cookieRoles);
            return encTicketAuth != null ? encTicketAuth.Length : 0;
        }

        public static void UpdateRolesCookie(this HttpResponseBase responseBase, string roles)
        {
            var cookieRoles = HttpContext.Current.Request.Cookies[".ASPXROLES"];
            if (cookieRoles == null)
            {
                return;
            }
            var ticketRoles = FormsAuthentication.Decrypt(cookieRoles.Value);
            if (ticketRoles == null)
            {
                return;
            }
            var newTicketRoles = new FormsAuthenticationTicket(ticketRoles.Version, ticketRoles.Name, ticketRoles.IssueDate, ticketRoles.Expiration,
                ticketRoles.IsPersistent, roles, "/ROLES");
            var encTickeRoles = FormsAuthentication.Encrypt(newTicketRoles);
            cookieRoles.Value = encTickeRoles;
            responseBase.Cookies.Add(cookieRoles);
        }
    }
}