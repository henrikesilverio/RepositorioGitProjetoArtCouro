using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Domain.Contracts.IService.IPessoa;
using ProjetoArtCouro.Model.Models.Common;

namespace ProjetoArtCouro.Api.Controllers.Pessoas
{
    [RoutePrefix("api/Pessoa")]
    public class PessoaController : ApiControllerBase
    {
        private readonly IPessoaService _pessoaService;
        public PessoaController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [Route("ObterListaEstado")]
        [HttpGet]
        public Task<HttpResponseMessage> ObterListaEstado()
        {
            HttpResponseMessage response;
            try
            {
                var listaEstado = _pessoaService.ObterEstados();
                response = ReturnSuccess(Mapper.Map<List<LookupModel>>(listaEstado));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("ObterListaEstadoCivil")]
        [HttpGet]
        public Task<HttpResponseMessage> ObterListaEstadoCivil()
        {
            HttpResponseMessage response;
            try
            {
                var listaEstadoCivil = _pessoaService.ObterEstadosCivis();
                response = ReturnSuccess(Mapper.Map<List<LookupModel>>(listaEstadoCivil));
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
            _pessoaService.Dispose();
        }
    }
}
