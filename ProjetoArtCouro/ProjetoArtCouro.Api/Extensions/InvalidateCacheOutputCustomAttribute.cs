using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Filters;
using MoreLinq;
using WebApi.OutputCache.V2;

namespace ProjetoArtCouro.Api.Extensions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class InvalidateCacheOutputCustomAttribute : BaseCacheAttribute
    {
        private readonly string _methodName;
        private string _controllerName;

        public InvalidateCacheOutputCustomAttribute(string methodName, string controllerName = null)
        {
            _methodName = methodName;
            _controllerName = controllerName;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null && !actionExecutedContext.Response.IsSuccessStatusCode) return;
            _controllerName = _controllerName ?? actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName;

            var config = actionExecutedContext.Request.GetConfiguration();
            EnsureCache(config, actionExecutedContext.Request);

            var partFinalKey = string.Format("{0}-{1}", _controllerName.ToLower(), _methodName.ToLower());
            var keys = WebApiCache.AllKeys.Where(x => x.Contains(partFinalKey));
            var enumerable = keys as IList<string> ?? keys.ToList();
            if (enumerable.Any())
            {
                enumerable.ForEach(x =>
                {
                    WebApiCache.RemoveStartsWith(x);
                });
            }
        }
    }
}