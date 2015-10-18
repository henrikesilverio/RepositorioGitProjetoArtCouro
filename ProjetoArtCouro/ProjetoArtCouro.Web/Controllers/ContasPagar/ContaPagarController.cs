using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.ContaPagar;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Authorization;

namespace ProjetoArtCouro.Web.Controllers.ContasPagar
{
    public class ContaPagarController : Controller
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

        [CustomAuthorize(Roles = "PesquisaContaPagar")]
        public JsonResult PesquisaContaPagar(PesquisaContaPagarModel model)
        {
            var resultado = new List<ContaPagarModel>
            {
                new ContaPagarModel
                {
                    CodigoCompra = 1,
                    CodigoFornecedor = 2,
                    DataEmissao = DateTime.Now,
                    DataVencimento = DateTime.Now.AddYears(1),
                    Status = "Aberto",
                    ValorDocumento = "R$ 300,00",
                    Recebido = false
                },
                new ContaPagarModel
                {
                    CodigoCompra = 2,
                    CodigoFornecedor = 6,
                    DataEmissao = DateTime.Now,
                    DataVencimento = DateTime.Now.AddYears(1),
                    Status = "Aberto",
                    ValorDocumento = "R$ 300,00",
                    Recebido = false
                },
                new ContaPagarModel
                {
                    CodigoCompra = 5,
                    CodigoFornecedor = 6,
                    DataEmissao = DateTime.Now,
                    DataVencimento = DateTime.Now.AddYears(1),
                    Status = "Recebido",
                    ValorDocumento = "R$ 300,00",
                    Recebido = true
                }
            };

            return Json(new { ObjetoRetorno = resultado }, JsonRequestBehavior.AllowGet);
        }
    }
}