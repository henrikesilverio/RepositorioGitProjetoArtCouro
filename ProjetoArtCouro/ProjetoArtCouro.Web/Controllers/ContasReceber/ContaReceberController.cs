using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.ContaReceber;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Authorization;

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
            var resultado = new List<ContaReceberModel>
            {
                new ContaReceberModel
                {
                    CodigoVenda = 1,
                    CodigoCliente = 2,
                    DataEmissao = DateTime.Now,
                    DataVencimento = DateTime.Now.AddYears(1),
                    Status = "Aberto",
                    ValorDocumento = "R$ 500,00",
                    Recebido = false
                },
                new ContaReceberModel
                {
                    CodigoVenda = 2,
                    CodigoCliente = 6,
                    DataEmissao = DateTime.Now,
                    DataVencimento = DateTime.Now.AddYears(1),
                    Status = "Recebido",
                    ValorDocumento = "R$ 600,00",
                    Recebido = true
                },
                new ContaReceberModel
                {
                    CodigoVenda = 5,
                    CodigoCliente = 6,
                    DataEmissao = DateTime.Now,
                    DataVencimento = DateTime.Now.AddYears(1),
                    Status = "Recebido",
                    ValorDocumento = "R$ 1000,00",
                    Recebido = true
                }
            };

            return Json(new { ObjetoRetorno = resultado }, JsonRequestBehavior.AllowGet);
        }
    }
}