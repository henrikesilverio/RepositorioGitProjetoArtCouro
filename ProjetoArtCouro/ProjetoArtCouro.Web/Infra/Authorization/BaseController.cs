using System;
using System.Net;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Extensions;
using RestSharp;

namespace ProjetoArtCouro.Web.Infra.Authorization
{
    [DeflateCompression]
    [GlobalErrorHandler]
    public class BaseController : Controller
    {
        public JsonResult ReturnResponse<T>(IRestResponse<RetornoBase<T>> response)
        {
            HttpContext.Response.Clear();
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Json(response.Data, JsonRequestBehavior.AllowGet);
                case HttpStatusCode.Unauthorized:
                    throw new Exception(Erros.UnauthorizedAccess);
                default:
                    HttpContext.Response.TrySkipIisCustomErrors = true;
                    HttpContext.Response.StatusCode = (int) response.StatusCode;
                    return Json(new {message = response.Data.Mensagem}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}