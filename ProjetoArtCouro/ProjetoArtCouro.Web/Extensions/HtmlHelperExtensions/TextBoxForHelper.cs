using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ProjetoArtCouro.Web.Extensions.HtmlHelperExtensions
{
    public static class TextBoxForHelper
    {
        public static MvcHtmlString TextBoxForCuston<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            while (true)
            {
                var metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
                // in .NET 4.5 you can use the new GetCustomAttribute<T>() method to check
                // for a single instance of the attribute, so this could be slightly
                // simplified to:
                // var attr = metaData.ContainerType.GetProperty(metaData.PropertyName)
                //                    .GetCustomAttribute<ReadOnly>();
                // if (attr != null)
                var isReadOnly = metaData.ContainerType.GetProperty(metaData.PropertyName).GetCustomAttributes(typeof (HtmlPropertiesAttribute), false).Any();

                if (isReadOnly) return helper.TextBoxFor(expression, new {@readonly = "readonly"});
            }
        }
    }
}