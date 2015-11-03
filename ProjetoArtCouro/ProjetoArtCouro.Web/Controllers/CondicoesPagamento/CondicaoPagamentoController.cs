using System.Collections.Generic;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.CondicaoPagamento;
using ProjetoArtCouro.Web.Infra.Authorization;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.CondicoesPagamento
{
    public class CondicaoPagamentoController : BaseController
    {
        [CustomAuthorize(Roles = "PesquisaCondicaoPagamento")]
        public ActionResult PesquisaCondicaoPagamento()
        {
            return View();
        }

        [CustomAuthorize(Roles = "PesquisaCondicaoPagamento")]
        public JsonResult ObterListaCondicaoPagamento()
        {
            var response = ServiceRequest.Get<List<CondicaoPagamentoModel>>("api/CondicaoPagamento/ObterListaCondicaoPagamento");
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "NovaCondicaoPagamento")]
        public JsonResult NovaCondicaoPagamento(CondicaoPagamentoModel model)
        {
            var response = ServiceRequest.Post<CondicaoPagamentoModel>(model, "api/CondicaoPagamento/CriarCondicaoPagamento");
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "EditarCondicaoPagamento")]
        public JsonResult EditarCondicaoPagamento(CondicaoPagamentoModel model)
        {
            var response = ServiceRequest.Put<object>(model, "api/CondicaoPagamento/EditarCondicaoPagamento");
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "ExcluirCondicaoPagamento")]
        public JsonResult ExcluirCondicaoPagamento(int codigoCondicaoPagamento)
        {
            var response = ServiceRequest.Delete<object>(new { codigoCondicaoPagamento = codigoCondicaoPagamento },
                "api/CondicaoPagamento/ExcluirCondicaoPagamento");
            return ReturnResponse(response);
        }
    }
}