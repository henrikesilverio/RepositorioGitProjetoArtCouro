using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Newtonsoft.Json.Linq;
using ProjetoArtCouro.Api.Extensions;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Domain.Contracts.IService.IPessoa;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Model.Models.Fornecedor;

namespace ProjetoArtCouro.Api.Controllers.Pessoas
{
    [RoutePrefix("api/Fornecedor")]
    public class FornecedorController : BaseApiController
    {
        private readonly IPessoaService _pessoaService;
        public FornecedorController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [Route("CriarFornecedor")]
        [Authorize(Roles = "NovoFornecedor")]
        [InvalidateCacheOutputCustom("ObterListaPessoa", "PessoaController")]
        [HttpPost]
        public Task<HttpResponseMessage> CriarFornecedor(FornecedorModel model)
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

        [Route("PesquisarFornecedor")]
        [Authorize(Roles = "PesquisaFornecedor")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarFornecedor(PesquisaFornecedorModel model)
        {
            HttpResponseMessage response;
            try
            {
                if (model.EPessoaFisica)
                {
                    var listaPessoaFisica = _pessoaService.PesquisarPessoaFisica(model.CodigoFornecedor ?? 0, model.Nome,
                    model.CPFCNPJ, model.Email, TipoPapelPessoaEnum.Fornecedor);
                    response = ReturnSuccess(Mapper.Map<List<FornecedorModel>>(listaPessoaFisica));
                }
                else
                {
                    var listaPessoaJuridica = _pessoaService.PesquisarPessoaJuridica(model.CodigoFornecedor ?? 0, model.Nome,
                    model.CPFCNPJ, model.Email, TipoPapelPessoaEnum.Fornecedor);
                    response = ReturnSuccess(Mapper.Map<List<FornecedorModel>>(listaPessoaJuridica));
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

        [Route("PesquisarFornecedorPorCodigo")]
        [Authorize(Roles = "EditarFornecedor")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarFornecedorPorCodigo([FromBody]JObject jObject)
        {
            var codigoFornecedor = jObject["codigoFornecedor"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                var pessoa = _pessoaService.ObterPessoaPorCodigo(codigoFornecedor);
                response =
                    ReturnSuccess(pessoa.PessoaFisica != null
                        ? Mapper.Map<FornecedorModel>(pessoa.PessoaFisica)
                        : Mapper.Map<FornecedorModel>(pessoa.PessoaJuridica));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("EditarFornecedor")]
        [Authorize(Roles = "EditarFornecedor")]
        [InvalidateCacheOutputCustom("ObterListaPessoa", "PessoaController")]
        [HttpPut]
        public Task<HttpResponseMessage> EditarFornecedor(FornecedorModel model)
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

        [Route("ExcluirFornecedor")]
        [Authorize(Roles = "ExcluirFornecedor")]
        [InvalidateCacheOutputCustom("ObterListaPessoa", "PessoaController")]
        [HttpDelete]
        public Task<HttpResponseMessage> ExcluirFornecedor([FromBody]JObject jObject)
        {
            var codigoFornecedor = jObject["codigoFornecedor"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                _pessoaService.ExcluirPessoa(codigoFornecedor);
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
