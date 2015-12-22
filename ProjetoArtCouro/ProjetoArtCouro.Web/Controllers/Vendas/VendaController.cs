using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Cliente;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.CondicaoPagamento;
using ProjetoArtCouro.Model.Models.FormaPagamento;
using ProjetoArtCouro.Model.Models.Produto;
using ProjetoArtCouro.Model.Models.Venda;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Authorization;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Vendas
{
    public class VendaController : BaseController
    {
        [CustomAuthorize(Roles = "PesquisaVenda")]
        public ActionResult PesquisaVenda()
        {
            ViewBag.Title = Mensagens.Sale;
            ViewBag.SubTitle = Mensagens.SearchSale;
            ViewBag.StatusVenda = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "Aberto"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Confirmado"
                },
                new LookupModel
                {
                    Codigo = 3,
                    Nome = "Cancelado"
                }
            };
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "PesquisaVenda")]
        public JsonResult PesquisaVenda(PesquisaVendaModel model)
        {
            //var response = ServiceRequest.Post<List<VendaModel>>(model, "api/Venda/PesquisaVenda");
            //return Json(response.Data, JsonRequestBehavior.AllowGet);
            var resultado = new List<VendaModel>
            {
                new VendaModel()
                {
                   CodigoVenda = 1,
                   ClienteId = 2,
                   DataCadastro = DateTime.Now,
                   Status = "Aberto",
                   NomeCliente = "Vitor",
                   CPFCNPJ = "123.456.789-09"
                },
                new VendaModel()
                {
                   CodigoVenda = 3,
                   ClienteId = 5,
                   DataCadastro = DateTime.Now,
                   Status = "Aberto",
                   NomeCliente = "Felipe",
                   CPFCNPJ = "222.333.666-38" 
                }
            };
            return Json(new {ObjetoRetorno = resultado}, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = "NovaVenda")]
        public ActionResult NovaVenda()
        {
            ViewBag.Title = Mensagens.Sale;
            ViewBag.SubTitle = Mensagens.NewSale;
            ViewBag.Produtos = new List<LookupModel>();
            var model = new VendaModel
            {
                Status = "Aberto",
                DataCadastro = DateTime.Now,
                ValorTotalBruto = "0,00",
                ValorTotalLiquido = "0,00",
                ValorTotalDesconto = "0,00"
            };
            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "NovaVenda")]
        public JsonResult NovaVenda(VendaModel model)
        {
            var response = ServiceRequest.Post<RetornoBase<string>>(model, "api/Venda/CriarVenda");
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = "EditarVenda")]
        public ActionResult EditarVenda(int codigoVenda)
        {
            ViewBag.Title = Mensagens.Sale;
            ViewBag.SubTitle = Mensagens.EditSale;
            ViewBag.Clientes = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "Vitor"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Henrique"
                },
                new LookupModel
                {
                    Codigo = 3,
                    Nome = "Andressa"
                },
                 new LookupModel
                {
                    Codigo = 4,
                    Nome = "Felipe"
                }
            };

            ViewBag.FormasPagamento = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "Cartão"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Dinheiro"
                },
                new LookupModel
                {
                    Codigo = 3,
                    Nome = "Cheque"
                }
            };

            ViewBag.CondicoesPagamento = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "A vista"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "1 + 1"
                },
                new LookupModel
                {
                    Codigo = 3,
                    Nome = "1 + 2"
                },
                 new LookupModel
                {
                    Codigo = 4,
                    Nome = "1 + 3"
                }
            };

            ViewBag.Produtos = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "Cinto de couro"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Bolsa de couro"
                }
            };

            var model = new VendaModel
            {
                Status = "Aberto",
                DataCadastro = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "EditarVenda")]
        public ActionResult EditarVenda(VendaModel model)
        {
            ViewBag.Title = Mensagens.NewSale;
            ViewBag.SubTitle = Mensagens.EditSale;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = ServiceRequest.Post<RetornoBase<string>>(model, "api/Venda/PesquisarVenda");
            if (!response.Data.TemErros)
            {
                return RedirectToAction("Index", "Venda");
            }
            ModelState.AddModelError("Erro", response.Data.Mensagem);

            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "ExcluirVenda")]
        public JsonResult ExcluirVenda(int codigoVenda)
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = "NovaVenda")]
        public JsonResult ObterListaCliente()
        {
            var response = ServiceRequest.Get<List<ClienteModel>>("api/Cliente/ObterListaCliente");
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "NovaVenda")]
        public JsonResult ObterListaFormasPagamento()
        {
            var response = ServiceRequest.Get<List<FormaPagamentoModel>>("api/FormaPagamento/ObterListaFormaPagamento");
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "NovaVenda")]
        public JsonResult ObterListaCondicoesPagamento()
        {
            var response = ServiceRequest.Get<List<CondicaoPagamentoModel>>("api/CondicaoPagamento/ObterListaCondicaoPagamento");
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "NovaVenda")]
        public JsonResult ObterListaProduto()
        {
            //TODO obter do estoque
            var response = ServiceRequest.Get<List<ProdutoModel>>("api/Produto/ObterListaProduto");
            return ReturnResponse(response);
        }
    }
}