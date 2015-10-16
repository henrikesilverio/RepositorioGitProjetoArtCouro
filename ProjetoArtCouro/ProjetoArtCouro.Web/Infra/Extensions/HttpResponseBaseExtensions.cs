using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

namespace ProjetoArtCouro.Web.Infra.Extensions
{
    public static class HttpResponseBaseExtensions
    {
        public static int SetAuthCookie<T>(this HttpResponseBase responseBase, string name, bool rememberMe, T userData)
        {
            var cookie = FormsAuthentication.GetAuthCookie(name, rememberMe);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            if (ticket == null)
            {
                return 0;
            }
            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                ticket.IsPersistent, JsonConvert.SerializeObject(userData), ticket.CookiePath);
            var encTicket = FormsAuthentication.Encrypt(newTicket);
            cookie.Value = encTicket;
            responseBase.Cookies.Add(cookie);
            return encTicket != null ? encTicket.Length : 0;
        }
    }
}