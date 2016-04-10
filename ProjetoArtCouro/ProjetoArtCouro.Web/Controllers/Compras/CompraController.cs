using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Compra;
using ProjetoArtCouro.Model.Models.CondicaoPagamento;
using ProjetoArtCouro.Model.Models.FormaPagamento;
using ProjetoArtCouro.Model.Models.Fornecedor;
using ProjetoArtCouro.Model.Models.Produto;
using ProjetoArtCouro.Resources.Resources;
using ProjetoArtCouro.Web.Infra.Authorization;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Compras
{
    public class CompraController : BaseController
    {
        [CustomAuthorize(Roles = "PesquisaCompra")]
        public ActionResult PesquisaCompra()
        {
            ViewBag.Title = Mensagens.Buy;
            ViewBag.SubTitle = Mensagens.SearchBuy;
            ViewBag.StatusCompra = new List<LookupModel>
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
        [CustomAuthorize(Roles = "PesquisaCompra")]
        public JsonResult PesquisaCompra(PesquisaCompraModel model)
        {
            var response = ServiceRequest.Post<List<CompraModel>>(model, "api/Compra/PesquisarCompra");
            if (response.Data.ObjetoRetorno != null && !response.Data.ObjetoRetorno.Any())
            {
                response.Data.Mensagem = Erros.NoShoppingForTheGivenFilter;
            }
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "NovaCompra")]
        public ActionResult NovaCompra()
        {
            ViewBag.Title = Mensagens.Buy;
            ViewBag.SubTitle = Mensagens.NewBuy;
            ViewBag.Produtos = new List<LookupModel>();
            var model = new CompraModel
            {
                StatusCompra = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "0,00",
                ValorTotalLiquido = "0,00",
                ValorTotalFrete = "0,00"
            };
            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "NovaCompra")]
        public JsonResult NovaCompra(CompraModel model)
        {
            var response = ServiceRequest.Post<RetornoBase<object>>(model, "api/Compra/CriarCompra");
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "EditarCompra")]
        public ActionResult EditarCompra(int codigoCompra)
        {
            ViewBag.Title = Mensagens.Buy;
            ViewBag.SubTitle = Mensagens.EditBuy;
            ViewBag.Produtos = new List<LookupModel>();
            var response = ServiceRequest.Post<CompraModel>(new { codigoCompra = codigoCompra }, "api/Compra/PesquisarCompraPorCodigo");
            var model = response.Data.ObjetoRetorno;
            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "EditarCompra")]
        public ActionResult EditarCompra(CompraModel model)
        {
            var response = ServiceRequest.Put<RetornoBase<object>>(model, "api/Compra/EditarCompra");
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "ExcluirCompra")]
        public JsonResult ExcluirCompra(int codigoCompra)
        {
            var response = ServiceRequest.Delete<RetornoBase<object>>(new { codigoCompra = codigoCompra }, "api/Compra/ExcluirCompra");
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "NovaCompra")]
        public JsonResult ObterListaFornecedor()
        {
            var response = ServiceRequest.Get<List<FornecedorModel>>("api/Fornecedor/ObterListaFornecedor");
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "NovaCompra")]
        public JsonResult ObterListaFormasPagamento()
        {
            var response = ServiceRequest.Get<List<FormaPagamentoModel>>("api/FormaPagamento/ObterListaFormaPagamento");
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "NovaCompra")]
        public JsonResult ObterListaCondicoesPagamento()
        {
            var response = ServiceRequest.Get<List<CondicaoPagamentoModel>>("api/CondicaoPagamento/ObterListaCondicaoPagamento");
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "NovaCompra")]
        public JsonResult ObterListaProduto()
        {
            var response = ServiceRequest.Get<List<ProdutoModel>>("api/Produto/ObterListaProduto");
            return ReturnResponse(response);
        }
    }
}