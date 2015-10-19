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
            
            var cookieRoles = new HttpCookie(".ASPXROLES");
            var ticketRoles = new FormsAuthenticationTicket(ticketAuth.Version, userName, ticketAuth.IssueDate, ticketAuth.Expiration,
                ticketAuth.IsPersistent, roles, "/ROLES");
            var encTickeRoles = FormsAuthentication.Encrypt(ticketRoles);
            cookieRoles.Value = encTickeRoles;

            var cookieToken = new HttpCookie(".ASPXTOKEN")
            {
                Value = EncryptionMD5.Encrypt(token),
                Expires = ticketAuth.Expiration
            };

            responseBase.Cookies.Add(cookieAuth);
            responseBase.Cookies.Add(cookieToken);
            responseBase.Cookies.Add(cookieRoles);
            return encTicketAuth != null ? encTicketAuth.Length : 0;
        }
    }
}