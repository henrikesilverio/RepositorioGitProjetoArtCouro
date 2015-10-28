using System.Web.Mvc;
using ProjetoArtCouro.Web.Infra.Authorization;

namespace ProjetoArtCouro.Web.Controllers.FormasPagamento
{
    public class FormaPagamentoController : BaseController
    {
        [CustomAuthorize(Roles = "PesquisaFormaPagamento")]
        public ActionResult PesquisaFormaPagamento()
        {
            return View();
        }
    }
}