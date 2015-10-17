﻿using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

namespace ProjetoArtCouro.Web.Infra.Extensions
{
    public static class HttpResponseBaseExtensions
    {
        public static int SetAuthCookie(this HttpResponseBase responseBase, string name, bool rememberMe, string token, string roles)
        {
            var cookie = FormsAuthentication.GetAuthCookie(name, rememberMe);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            if (ticket == null)
            {
                return 0;
            }
            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                ticket.IsPersistent, JsonConvert.SerializeObject(token), ticket.CookiePath);
            var encTicket = FormsAuthentication.Encrypt(newTicket);
            cookie.Value = encTicket;
            
            var cookieRoles = new HttpCookie(".ASPXROLES");
            var ticketRoles = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                ticket.IsPersistent, roles, ticket.CookiePath);
            var encTickeRoles = FormsAuthentication.Encrypt(ticketRoles);
            cookieRoles.Value = encTickeRoles;

            responseBase.Cookies.Add(cookie);
            responseBase.Cookies.Add(cookieRoles);
            return encTicket != null ? encTicket.Length : 0;
        }
    }
}