using System;
using System.Collections.Generic;
using System.Linq;
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
            var response = ServiceRequest.Post<List<VendaModel>>(model, "api/Venda/PesquisarVenda");
            if (response.Data.ObjetoRetorno != null && !response.Data.ObjetoRetorno.Any())
            {
                response.Data.Mensagem = Erros.NoSaleForTheGivenFilter;
            }
            return ReturnResponse(response);
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
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm:ss}", DateTime.Now),
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
            var response = ServiceRequest.Post<RetornoBase<object>>(model, "api/Venda/CriarVenda");
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "EditarVenda")]
        public ActionResult EditarVenda(int codigoVenda)
        {
            ViewBag.Title = Mensagens.Sale;
            ViewBag.SubTitle = Mensagens.EditSale;
            ViewBag.Produtos = new List<LookupModel>();
            var response = ServiceRequest.Post<VendaModel>(new { codigoVenda = codigoVenda }, "api/Venda/PesquisarVendaPorCodigo");
            var model = response.Data.ObjetoRetorno;
            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "EditarVenda")]
        public JsonResult EditarVenda(VendaModel model)
        {
            var response = ServiceRequest.Put<RetornoBase<object>>(model, "api/Venda/EditarVenda");
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "ExcluirVenda")]
        public JsonResult ExcluirVenda(int codigoVenda)
        {
            var response = ServiceRequest.Delete<RetornoBase<object>>(new { codigoVenda = codigoVenda }, "api/Venda/ExcluirVenda");
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "NovaVenda")]
        public JsonResult ObterListaCliente()
        {
            var response = ServiceRequest.Get<List<PessoaModel>>("api/Pessoa/ObterListaPessoa");
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
            var response = ServiceRequest.Get<List<ProdutoModel>>("api/Produto/ObterListaProduto");
            return ReturnResponse(response);
        }
    }
}