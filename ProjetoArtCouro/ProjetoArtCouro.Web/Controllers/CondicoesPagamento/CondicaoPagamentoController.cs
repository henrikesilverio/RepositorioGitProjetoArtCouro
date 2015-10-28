using System.Web.Mvc;
using ProjetoArtCouro.Web.Infra.Authorization;

namespace ProjetoArtCouro.Web.Controllers.CondicoesPagamento
{
    public class CondicaoPagamentoController : BaseController
    {
        [CustomAuthorize(Roles = "PesquisaCondicaoPagamento")]
        public ActionResult PesquisaCondicaoPagamento()
        {
            return View();
        }
    }
}