using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.ContaPagar;
using ProjetoArtCouro.Resources.Resources;
using ProjetoArtCouro.Web.Infra.Authorization;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.ContasPagar
{
    public class ContaPagarController : BaseController
    {
        [CustomAuthorize(Roles = "PesquisaContaPagar")]
        public ActionResult PesquisaContaPagar()
        {
            ViewBag.Title = Mensagens.BillsToPay;
            ViewBag.StatusContaPagar = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "Aberto"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Recebido"
                }
            };

            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "PesquisaContaPagar")]
        public JsonResult PesquisaContaPagar(PesquisaContaPagarModel model)
        {
            var response = ServiceRequest.Post<List<ContaPagarModel>>(model, "api/ContaPagar/PesquisaContaPagar");
            if (response.Data.ObjetoRetorno != null && !response.Data.ObjetoRetorno.Any())
            {
                response.Data.Mensagem = Erros.NoAccountReceivableForTheGivenFilter;
            }
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "PesquisaContaPagar")]
        public JsonResult PagarConta(List<ContaPagarModel> model)
        {
            var response = ServiceRequest.Put<RetornoBase<object>>(model, "api/ContaPagar/PagarConta");
            return ReturnResponse(response);
        }
    }
}