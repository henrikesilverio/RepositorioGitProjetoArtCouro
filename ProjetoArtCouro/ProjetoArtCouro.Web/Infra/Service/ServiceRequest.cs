using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using ProjetoArtCouro.Model.Models.Common;
using RestSharp;

namespace ProjetoArtCouro.Web.Infra.Service
{
    public static class ServiceRequest
    {
        public static TokenModel GetAuthenticationToken(string userName, string password)
        {
            var client = new RestClient("http://localhost:5839");
            var request = new RestRequest("/api/security/token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", userName);
            request.AddParameter("password", password);

            var response = client.Execute<TokenModel>(request);
            if (response.Data == null)
            {
                return null;
            }
            var token = response.Data.access_token;
            return string.IsNullOrEmpty(token) ? null : response.Data;
        }

        public static IRestResponse<RetornoBase<T>> Post<T>(object objectParameter, string apiEndPoint)
            where T : new()
        {
            return ExecuteAction<T>(objectParameter, apiEndPoint, Method.POST);
        }

        public static IRestResponse<RetornoBase<T>> Put<T>(object objectParameter, string apiEndPoint)
            where T : new()
        {
            return ExecuteAction<T>(objectParameter, apiEndPoint, Method.PUT);
        }

        public static IRestResponse<RetornoBase<T>> Get<T>(object objectParameter, string apiEndPoint)
            where T : new()
        {
            return ExecuteAction<T>(objectParameter, apiEndPoint, Method.GET);
        }

        public static IRestResponse<RetornoBase<T>> Delete<T>(object objectParameter, string apiEndPoint)
            where T : new()
        {
            return ExecuteAction<T>(objectParameter, apiEndPoint, Method.DELETE);
        }

        private static IRestResponse<RetornoBase<T>> ExecuteAction<T>(object objectParameter, string apiEndPoint, Method method) 
            where T : new()
        {
            var client = new RestClient("http://localhost:5839");
            var request = new RestRequest(apiEndPoint, method);
            var json = JsonConvert.SerializeObject(objectParameter);            
            request.AddParameter("text/json", json, ParameterType.RequestBody);
            var token = GetTokenForAuthCookie();
            request.AddHeader("Authorization", string.Format("Bearer {0}", token));
            var response = client.Execute<RetornoBase<T>>(request);
            return response;
        }

        private static string GetTokenForAuthCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null)
            {
                return string.Empty;
            }
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            TokenModel tokenModel = null;
            if (ticket != null)
            {
                tokenModel = JsonConvert.DeserializeObject<TokenModel>(ticket.UserData);
            }
            return tokenModel != null ? tokenModel.access_token : string.Empty;
        }
    }
}