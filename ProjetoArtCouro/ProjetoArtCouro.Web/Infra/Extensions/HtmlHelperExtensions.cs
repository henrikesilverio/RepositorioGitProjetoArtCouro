using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ProjetoArtCouro.Web.Infra.WebValidator;

namespace ProjetoArtCouro.Web.Infra.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(
           this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> expression,
           IEnumerable<SelectListItem> listOfValues, string htmlClass = "")
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var sb = new StringBuilder();
            var htmlAttributes = new Dictionary<string, object>();
            if (HasAttributesToAdd(htmlHelper, expression))
            {
                htmlAttributes = GetHtmlAttributes(htmlHelper, expression);
            }

            if (!string.IsNullOrEmpty(htmlClass))
            {
                htmlAttributes.Add("class", htmlClass);
            }

            if (listOfValues == null)
            {
                return MvcHtmlString.Create(sb.ToString());
            }

            foreach (var item in listOfValues)
            {
                var id = string.Format(
                    "{0}",
                    metaData.PropertyName,
                    item.Value
                    );
                var ret = new Dictionary<string, object>(htmlAttributes) { { "id", id } };

                var radio = htmlHelper.RadioButtonFor(expression, item.Value, ret).ToHtmlString();
                sb.AppendFormat(
                    "<label class='radio'>{2}<i></i>{1}</label>",
                    id,
                    HttpUtility.HtmlEncode(item.Text),
                    radio
                    );
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        private static bool HasAttributesToAdd<TModel, TValue>(HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            var metaData = GetModelMetadata(helper, expression);
            return metaData.ContainerType.GetProperty(metaData.PropertyName)
                .GetCustomAttributes(typeof(HtmlPropertiesAttribute), false)
                .Any();
        }

        private static ModelMetadata GetModelMetadata<TModel, TValue>(HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            return ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
        }

        private static Dictionary<string, object> GetHtmlAttributes<TModel, TValue>(HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            var metaData = GetModelMetadata(helper, expression);
            var htmlPropertiesAttribute = (HtmlPropertiesAttribute)metaData.ContainerType.GetProperty(metaData.PropertyName)
                        .GetCustomAttributes(typeof(HtmlPropertiesAttribute), false)[0];
            return (Dictionary<string, object>)htmlPropertiesAttribute.GetHtmlAttributes();
        }
    }
}