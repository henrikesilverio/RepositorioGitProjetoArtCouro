using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Newtonsoft.Json.Linq;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Domain.Contracts.IService.IProduto;
using ProjetoArtCouro.Domain.Models.Produtos;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Produto;
using WebApi.OutputCache.V2;

namespace ProjetoArtCouro.Api.Controllers.Produtos
{
    [RoutePrefix("api/Produto")]
    public class ProdutoController : BaseApiController
    {
        private readonly IProdutoService _produtoService;
        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [Route("ObterListaProduto")]
        [Authorize(Roles = "PesquisaProduto, NovaVenda")]
        [CacheOutput(ServerTimeSpan = 10000)]
        [HttpGet]
        public Task<HttpResponseMessage> ObterListaProduto()
        {
            HttpResponseMessage response;
            try
            {
                var listaProduto = _produtoService.ObterListaProduto();
                response = ReturnSuccess(Mapper.Map<List<ProdutoModel>>(listaProduto));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("ObterListaUnidade")]
        [Authorize(Roles = "PesquisaProduto")]
        [CacheOutput(ServerTimeSpan = 10000)]
        [HttpGet]
        public Task<HttpResponseMessage> ObterListaUnidade()
        {
            HttpResponseMessage response;
            try
            {
                var listaUnidade = _produtoService.ObterListaUnidade();
                response = ReturnSuccess(Mapper.Map<List<LookupModel>>(listaUnidade));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("CriarProduto")]
        [Authorize(Roles = "NovoProduto")]
        [InvalidateCacheOutput("ObterListaProduto")]
        [HttpPost]
        public Task<HttpResponseMessage> CriarProduto(ProdutoModel model)
        {
            HttpResponseMessage response;
            try
            {
                var produto = Mapper.Map<Produto>(model);
                var produtoRetorno = _produtoService.CriarProduto(produto);
                response = ReturnSuccess(Mapper.Map<ProdutoModel>(produtoRetorno));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("EditarProduto")]
        [Authorize(Roles = "EditarProduto")]
        [InvalidateCacheOutput("ObterListaProduto")]
        [HttpPut]
        public Task<HttpResponseMessage> EditarProduto(ProdutoModel model)
        {
            HttpResponseMessage response;
            try
            {
                var produto = Mapper.Map<Produto>(model);
                var produtoRetorno = _produtoService.AtualizarProduto(produto);
                response = ReturnSuccess(Mapper.Map<ProdutoModel>(produtoRetorno));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("ExcluirProduto")]
        [Authorize(Roles = "ExcluirProduto")]
        [InvalidateCacheOutput("ObterListaProduto")]
        [HttpDelete]
        public Task<HttpResponseMessage> ExcluirProduto([FromBody]JObject jObject)
        {
            var codigoProduto = jObject["codigoProduto"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                _produtoService.ExcluirProduto(codigoProduto);
                response = ReturnSuccess();
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        protected override void Dispose(bool disposing)
        {
            _produtoService.Dispose();
        }
    }
}
