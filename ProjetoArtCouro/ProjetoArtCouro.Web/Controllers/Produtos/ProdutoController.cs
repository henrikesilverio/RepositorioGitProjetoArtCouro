using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Produto;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Authorization;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Produtos
{
    public class ProdutoController : BaseController
    {
        [CustomAuthorize(Roles = "PesquisaProduto")]
        public ActionResult PesquisaProduto()
        {
            ViewBag.Title = Mensagens.Product;
            var response = ServiceRequest.Get<List<LookupModel>>("api/Produto/ObterListaUnidade");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                ViewBag.Unidades = new List<LookupModel>();
                throw new Exception(Erros.CouldNotLoadUnits);
            }
            ViewBag.Unidades = response.Data.ObjetoRetorno;
            return View();
        }

        [CustomAuthorize(Roles = "PesquisaProduto")]
        public JsonResult ObterListaProduto()
        {
            var response = ServiceRequest.Get<List<ProdutoModel>>("api/Produto/ObterListaProduto");
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "NovoProduto")]
        public JsonResult NovoProduto(ProdutoModel model)
        {
            var response = ServiceRequest.Post<ProdutoModel>(model, "api/Produto/CriarProduto");
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "EditarProduto")]
        public JsonResult EditarProduto(ProdutoModel model)
        {
            var response = ServiceRequest.Put<object>(model, "api/Produto/EditarProduto");
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "ExcluirProduto")]
        public JsonResult ExcluirProduto(int codigoProduto)
        {
            var response = ServiceRequest.Delete<object>(new {codigoProduto = codigoProduto},
                "api/Produto/ExcluirProduto");
            return ReturnResponse(response);
        }
    }
}