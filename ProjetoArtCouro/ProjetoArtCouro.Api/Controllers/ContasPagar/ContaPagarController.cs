using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Domain.Contracts.IService.ICompra;
using ProjetoArtCouro.Domain.Models.Compras;
using ProjetoArtCouro.Model.Models.ContaPagar;

namespace ProjetoArtCouro.Api.Controllers.ContasPagar
{
    [RoutePrefix("api/ContaPagar")]
    public class ContaPagarController : BaseApiController
    {
        private readonly IContaPagarService _contaPagarService;

        public ContaPagarController(IContaPagarService contaPagarService)
        {
            _contaPagarService = contaPagarService;
        }

        [Route("PesquisaContaPagar")]
        [Authorize(Roles = "PesquisaContaPagar")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisaContaPagar(PesquisaContaPagarModel model)
        {
            HttpResponseMessage response;
            try
            {
                var usuarioCodigo = ObterCodigoUsuarioLogado();
                var contasPagar = _contaPagarService.PesquisarContaPagar(model.CodigoCompra ?? 0, model.CodigoFornecedor ?? 0,
                    model.DataEmissao.ToDateTimeWithoutHour(), model.DataVencimento.ToDateTimeWithoutHour(),
                    model.StatusId ?? 0, model.NomeFornecedor, model.CPFCNPJ, usuarioCodigo);
                response = ReturnSuccess(Mapper.Map<List<ContaPagarModel>>(contasPagar));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("PagarConta")]
        [Authorize(Roles = "PesquisaContaPagar")]
        [HttpPut]
        public Task<HttpResponseMessage> PagarConta(List<ContaPagarModel> listaContaPagarModel)
        {
            HttpResponseMessage response;
            try
            {
                var listaContaPagar = Mapper.Map<List<ContaPagar>>(listaContaPagarModel);
                _contaPagarService.PagarContas(listaContaPagar);
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

        private static int ObterCodigoUsuarioLogado()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var usuarioCodigo = identity.Claims.Where(c => c.Type == ClaimTypes.Sid)
                .Select(c => c.Value).SingleOrDefault();
            return usuarioCodigo.ToInt();
        }

        protected override void Dispose(bool disposing)
        {
            _contaPagarService.Dispose();
        }
    }
}
