using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetoArtCouro.Web.Extensions.HtmlHelperExtensions
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString ValidationSummaryBootstrap(this HtmlHelper htmlHelper)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }

            if (htmlHelper.ViewData.ModelState.IsValid)
            {
                return new HtmlString(string.Empty);
            }

            var listErro = htmlHelper.ViewData.ModelState.Values.FirstOrDefault(x => x.Errors.Count > 0);
            if (listErro == null)
            {
                return null;
            }

            var listMessegeErros = "";
            if (htmlHelper.ViewData.ModelState.Keys.Any(x => x.Equals("Erro")))
            {
                listMessegeErros = listErro.Errors.Aggregate("", (current, erro) => current +
                ("<div class=\"alert alert-danger fade in\">" + 
                "<button class=\"close\" data-dismiss=\"alert\">×</button>" +
                "<i class=\"fa-fw fa fa-times\"></i>" +
                "<strong>Erro!</strong> " + erro.ErrorMessage + "</div>"));
            }
            else if (htmlHelper.ViewData.ModelState.Keys.Any(x => x.Equals("Atencao")))
            {
                listMessegeErros = listErro.Errors.Aggregate("", (current, erro) => current +
                ("<div class=\"alert alert-warning fade in\">" +
                "<button class=\"close\" data-dismiss=\"alert\">×</button>" +
                "<i class=\"fa-fw fa fa-warning\"></i>" +
                "<strong>Atenção</strong> " + erro.ErrorMessage + "</div>"));
            }

            return new HtmlString(listMessegeErros);
        }
    }
}