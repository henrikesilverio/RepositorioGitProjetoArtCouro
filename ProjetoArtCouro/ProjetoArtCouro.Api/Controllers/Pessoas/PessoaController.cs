using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using ProjetoArtCouro.Domain.Contracts.IService.IPessoa;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Api.Controllers.Pessoas
{
    [RoutePrefix("api/Pessoa")]
    public class PessoaController : ApiController
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
            var retornoBase = new RetornoBase()
            {
                Mensagem = Mensagens.ReturnSuccess,
                TemErros = false,
            };

            try
            {
                var listaEstado = _pessoaService.ObterEstados();
                retornoBase.ObjetoRetorno = Mapper.Map<List<LookupModel>>(listaEstado);
                response = Request.CreateResponse(HttpStatusCode.OK, retornoBase);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
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
            var retornoBase = new RetornoBase()
            {
                Mensagem = Mensagens.ReturnSuccess,
                TemErros = false,
            };

            try
            {
                var listaEstadoCivil = _pessoaService.ObterEstadosCivis();
                retornoBase.ObjetoRetorno = Mapper.Map<List<LookupModel>>(listaEstadoCivil);
                response = Request.CreateResponse(HttpStatusCode.OK, retornoBase);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}
