using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Newtonsoft.Json.Linq;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Domain.Contracts.IService.IPessoa;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Model.Models.Cliente;
using WebApi.OutputCache.V2;

namespace ProjetoArtCouro.Api.Controllers.Pessoas
{
    [RoutePrefix("api/Cliente")]
    public class ClienteController : BaseApiController
    {
        private readonly IPessoaService _pessoaService;
        public ClienteController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [Route("CriarCliente")]
        [Authorize(Roles = "NovoCliente")]
        [InvalidateCacheOutput("ObterListaCliente")]
        [HttpPost]
        public Task<HttpResponseMessage> CriarCliente(ClienteModel model)
        {
            HttpResponseMessage response;
            try
            {
                var pessoa = Mapper.Map<Pessoa>(model);
                pessoa.Papeis = new List<Papel> { new Papel { PapelCodigo = model.PapelPessoa } };
                //Remove informações que não vão ser gravadas.
                ((List<MeioComunicacao>)pessoa.MeiosComunicacao).RemoveAll(
                    x => string.IsNullOrEmpty(x.MeioComunicacaoNome) && x.MeioComunicacaoCodigo.Equals(0));

                if (model.EPessoaFisica)
                {
                    pessoa.PessoaFisica = Mapper.Map<PessoaFisica>(model);
                    _pessoaService.CriarPessoaFisica(pessoa);
                }
                else
                {
                    pessoa.PessoaJuridica = Mapper.Map<PessoaJuridica>(model);
                    _pessoaService.CriarPessoaJuridica(pessoa);
                }
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

        [Route("ObterListaCliente")]
        [Authorize(Roles = "NovaVenda")]
        [CacheOutput(ServerTimeSpan = 10000)]
        [HttpGet]
        public Task<HttpResponseMessage> ObterListaCliente()
        {
            HttpResponseMessage response;
            try
            {
                var listaPessoaFisica = _pessoaService.ObterListaPessoaFisicaPorPapel(TipoPapelPessoaEnum.Cliente);
                var listaPessoaJuridica = _pessoaService.ObterListaPessoaJuridicaPorPapel(TipoPapelPessoaEnum.Cliente);
                var listaCliente = Mapper.Map<List<ClienteModel>>(listaPessoaFisica);
                listaCliente.AddRange(Mapper.Map<List<ClienteModel>>(listaPessoaJuridica));
                response = ReturnSuccess(listaCliente);
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("PesquisarCliente")]
        [Authorize(Roles = "PesquisaCliente")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarCliente(PesquisaClienteModel model)
        {
            HttpResponseMessage response;
            try
            {
                if (model.EPessoaFisica)
                {
                    var listaPessoaFisica = _pessoaService.PesquisarPessoaFisica(model.CodigoCliente ?? 0, model.Nome,
                    model.CPFCNPJ, model.Email, TipoPapelPessoaEnum.Cliente);
                    response = ReturnSuccess(Mapper.Map<List<ClienteModel>>(listaPessoaFisica));
                }
                else
                {
                    var listaPessoaJuridica = _pessoaService.PesquisarPessoaJuridica(model.CodigoCliente ?? 0, model.Nome,
                    model.CPFCNPJ, model.Email, TipoPapelPessoaEnum.Cliente);
                    response = ReturnSuccess(Mapper.Map<List<ClienteModel>>(listaPessoaJuridica));
                }
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("PesquisarClientePorCodigo")]
        [Authorize(Roles = "EditarCliente")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarClientePorCodigo([FromBody]JObject jObject)
        {
            var codigoCliente = jObject["codigoCliente"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                var pessoa = _pessoaService.ObterPessoaPorCodigo(codigoCliente);
                response =
                    ReturnSuccess(pessoa.PessoaFisica != null
                        ? Mapper.Map<ClienteModel>(pessoa.PessoaFisica)
                        : Mapper.Map<ClienteModel>(pessoa.PessoaJuridica));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("EditarCliente")]
        [Authorize(Roles = "EditarCliente")]
        [InvalidateCacheOutput("ObterListaCliente")]
        [HttpPut]
        public Task<HttpResponseMessage> EditarCliente(ClienteModel model)
        {
            HttpResponseMessage response;

            try
            {
                var pessoa = Mapper.Map<Pessoa>(model);
                //Remove informações que não vão ser gravadas.
                ((List<MeioComunicacao>)pessoa.MeiosComunicacao).RemoveAll(
                    x => string.IsNullOrEmpty(x.MeioComunicacaoNome) && x.MeioComunicacaoCodigo.Equals(0));
                if (model.EPessoaFisica)
                {
                    pessoa.PessoaFisica = Mapper.Map<PessoaFisica>(model);
                }
                else
                {
                    pessoa.PessoaJuridica = Mapper.Map<PessoaJuridica>(model);
                }
                _pessoaService.AtualizarPessoa(pessoa);
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

        [Route("ExcluirCliente")]
        [Authorize(Roles = "ExcluirCliente")]
        [InvalidateCacheOutput("ObterListaCliente")]
        [HttpDelete]
        public Task<HttpResponseMessage> ExcluirCliente([FromBody]JObject jObject)
        {
            var codigoCliente = jObject["codigoCliente"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                _pessoaService.ExcluirPessoa(codigoCliente);
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
            _pessoaService.Dispose();
        }
    }
}
