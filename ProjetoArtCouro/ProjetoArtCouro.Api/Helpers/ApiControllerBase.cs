using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjetoArtCouro.Model.Models.Common;

namespace ProjetoArtCouro.Api.Helpers
{
    public class ApiControllerBase : ApiController
    {
        public HttpResponseMessage ReturnError(Exception ex)
        {
            var retornoBase = new RetornoBase<Exception>
            {
                ObjetoRetorno = ex,
                Mensagem = ex.Message,
                TemErros = true
            };
            return Request.CreateResponse(HttpStatusCode.BadRequest, retornoBase);
        }

        public HttpResponseMessage ReturnSuccess()
        {
            var retornoBase = new RetornoBase<object>();
            return Request.CreateResponse(HttpStatusCode.OK, retornoBase);
        }

        public HttpResponseMessage ReturnSuccess<T>(T objetReturn)
        {
            var retornoBase = new RetornoBase<T>
            {
                ObjetoRetorno = objetReturn
            };
            return Request.CreateResponse(HttpStatusCode.OK, retornoBase);
        }
    }
}