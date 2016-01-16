using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Domain.Contracts.IService.IEstoque;
using ProjetoArtCouro.Model.Models.Estoque;

namespace ProjetoArtCouro.Api.Controllers.Estoques
{
    [RoutePrefix("api/Estoque")]
    public class EstoqueController : BaseApiController
    {
        private readonly IEstoqueService _estoqueService;

        public EstoqueController(IEstoqueService estoqueService)
        {
            _estoqueService = estoqueService;
        }

        [Route("PesquisarEstoque")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarCompra(PesquisaEstoqueModel model)
        {
            HttpResponseMessage response;
            try
            {
                var estoques = _estoqueService.PesquisarEstoque(model.DescricaoProduto,
                    model.CodigoProduto.GetValueOrDefault(),
                    model.QuantidaEstoque.GetValueOrDefault(), model.NomeFornecedor,
                    model.CodigoFornecedor.GetValueOrDefault());
                response = ReturnSuccess(Mapper.Map<List<EstoqueModel>>(estoques));
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
            _estoqueService.Dispose();
        }
    }
}
