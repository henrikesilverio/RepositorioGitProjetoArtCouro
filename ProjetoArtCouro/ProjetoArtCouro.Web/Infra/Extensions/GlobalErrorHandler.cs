using System.Net;
using System.Web.Mvc;

namespace ProjetoArtCouro.Web.Infra.Extensions
{
    public class GlobalErrorHandler : FilterAttribute, IExceptionFilter
    {
        public virtual void OnException(ExceptionContext filterContext)
        {
            filterContext.HttpContext.Response.Clear();
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            filterContext.Result = new JsonResult
            {
                Data = new { message = filterContext.Exception.Message },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}