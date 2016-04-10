using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.ContaReceber;
using ProjetoArtCouro.Resources.Resources;
using ProjetoArtCouro.Web.Infra.Authorization;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.ContasReceber
{
    public class ContaReceberController : BaseController
    {
        [CustomAuthorize(Roles = "PesquisaContaReceber")]
        public ActionResult PesquisaContaReceber()
        {
            ViewBag.Title = Mensagens.BillsToPay;
            ViewBag.StatusContaReceber = new List<LookupModel>
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
        [CustomAuthorize(Roles = "PesquisaContaReceber")]
        public JsonResult PesquisaContaReceber(PesquisaContaReceberModel model)
        {
            var response = ServiceRequest.Post<List<ContaReceberModel>>(model, "api/ContaReceber/PesquisaContaReceber");
            if (response.Data.ObjetoRetorno != null && !response.Data.ObjetoRetorno.Any())
            {
                response.Data.Mensagem = Erros.NoAccountReceivableForTheGivenFilter;
            }
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "PesquisaContaReceber")]
        public JsonResult ReceberConta(List<ContaReceberModel> model)
        {
            var response = ServiceRequest.Put<RetornoBase<object>>(model, "api/ContaReceber/ReceberConta");
            return ReturnResponse(response);
        }
    }
}