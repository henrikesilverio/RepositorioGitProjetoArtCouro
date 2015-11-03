using System.Collections.Generic;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.FormaPagamento;
using ProjetoArtCouro.Web.Infra.Authorization;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.FormasPagamento
{
    public class FormaPagamentoController : BaseController
    {
        [CustomAuthorize(Roles = "PesquisaFormaPagamento")]
        public ActionResult PesquisaFormaPagamento()
        {
            return View();
        }

        [CustomAuthorize(Roles = "PesquisaFormaPagamento")]
        public JsonResult ObterListaFormaPagamento()
        {
            var response = ServiceRequest.Get<List<FormaPagamentoModel>>("api/FormaPagamento/ObterListaFormaPagamento");
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "NovaFormaPagamento")]
        public JsonResult NovaFormaPagamento(FormaPagamentoModel model)
        {
            var response = ServiceRequest.Post<FormaPagamentoModel>(model, "api/FormaPagamento/CriarFormaPagamento");
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "EditarFormaPagamento")]
        public JsonResult EditarFormaPagamento(FormaPagamentoModel model)
        {
            var response = ServiceRequest.Put<object>(model, "api/FormaPagamento/EditarFormaPagamento");
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "ExcluirFormaPagamento")]
        public JsonResult ExcluirFormaPagamento(int codigoFormaPagamento)
        {
            var response = ServiceRequest.Delete<object>(new { codigoFormaPagamento = codigoFormaPagamento },
                "api/FormaPagamento/ExcluirFormaPagamento");
            return ReturnResponse(response);
        }
    }
}