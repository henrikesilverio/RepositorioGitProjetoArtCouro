using System.Web.Security;
using Newtonsoft.Json;
using ProjetoArtCouro.Model.Models.Common;
using RestSharp;

namespace ProjetoArtCouro.Web.Infra.Service
{
    public static class ServiceRequest
    {
        public static bool SetAuthenticationToken(string userName, string password)
        {
            var client = new RestClient("http://localhost:5839");
            var request = new RestRequest("/api/security/token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", userName);
            request.AddParameter("password", password);

            var response = client.Execute<TokenModel>(request);
            var token = response.Data.access_token;

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            //Salva o token de acesso ao serviço no Cookie
            FormsAuthentication.SetAuthCookie(token, false);

            return true;
        }

        public static IRestResponse<RetornoBase> Post(object objectParameter, string apiEndPoint) 
        {
            return ExecuteAction(objectParameter, apiEndPoint, Method.POST);
        }

        public static IRestResponse<RetornoBase> Put(object objectParameter, string apiEndPoint) 
        {
            return ExecuteAction(objectParameter, apiEndPoint, Method.PUT);
        }

        public static IRestResponse<RetornoBase> Get(object objectParameter, string apiEndPoint)
        {
            return ExecuteAction(objectParameter, apiEndPoint, Method.GET);
        }

        public static IRestResponse<RetornoBase> Delete(object objectParameter, string apiEndPoint)
        {
            return ExecuteAction(objectParameter, apiEndPoint, Method.DELETE);
        }

        private static IRestResponse<RetornoBase> ExecuteAction(object objectParameter, string apiEndPoint, Method method)
        {
            //Falta pegar o token para enviar na requisição
            var client = new RestClient("http://localhost:5839");
            var request = new RestRequest(apiEndPoint, method);
            var json = JsonConvert.SerializeObject(objectParameter);
            request.AddParameter("text/json", json, ParameterType.RequestBody);
            var response = client.Execute<RetornoBase>(request);
            return response;
        }
    }
}