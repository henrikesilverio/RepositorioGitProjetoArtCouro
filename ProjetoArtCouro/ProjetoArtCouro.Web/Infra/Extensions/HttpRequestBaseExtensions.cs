using System.Security.Claims;
using System.Web;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace ProjetoArtCouro.Web.Infra.Extensions
{
    public static class HttpRequestBaseExtensions
    {
        public static void SignIn(this HttpRequestBase requestBase, string userName, string token, string roles, bool rememberMe)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.SerialNumber, token)
            }, "ApplicationCookie");
            UpdateRoles(identity, roles);
            var authManager = GetAuthManager(requestBase);
            authManager.SignIn(identity);
        }

        public static void SignOut(this HttpRequestBase requestBase)
        {
            var authManager = GetAuthManager(requestBase);
            authManager.SignOut("ApplicationCookie");
        }

        public static void UpdateRoles(this HttpRequestBase requestBase, string roles)
        {
            var authManager = GetAuthManager(requestBase);
            var arraryRoles = JsonConvert.DeserializeObject<string[]>(roles);
            var claimsIdentity = authManager.User.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return;
            }
            foreach (var role in arraryRoles)
            {
                claimsIdentity.AddClaim(
                    new Claim(ClaimTypes.Role, role));
            }
            authManager.SignIn(claimsIdentity);
        }

        private static void UpdateRoles(ClaimsIdentity identity, string roles)
        {
            var arraryRoles = JsonConvert.DeserializeObject<string[]>(roles);
            foreach (var role in arraryRoles)
            {
                identity.AddClaim(
                    new Claim(ClaimTypes.Role, role));
            }
        }

        private static IAuthenticationManager GetAuthManager(HttpRequestBase requestBase)
        {
            var ctx = requestBase.GetOwinContext();
            var authManager = ctx.Authentication;
            return authManager;
        }
    }
}