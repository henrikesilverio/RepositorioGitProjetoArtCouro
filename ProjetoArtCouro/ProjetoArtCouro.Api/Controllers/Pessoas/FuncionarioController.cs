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
using ProjetoArtCouro.Model.Models.Funcionario;

namespace ProjetoArtCouro.Api.Controllers.Pessoas
{
    [RoutePrefix("api/Funcionario")]
    public class FuncionarioController : ApiControllerBase
    {
        private readonly IPessoaService _pessoaService;
        public FuncionarioController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [Route("CriarFuncionario")]
        [HttpPost]
        public Task<HttpResponseMessage> CriarFuncionario(FuncionarioModel model)
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

        [Route("PesquisarFuncionario")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarFuncionario(PesquisaFuncionarioModel model)
        {
            HttpResponseMessage response;
            try
            {
                if (model.EPessoaFisica)
                {
                    var listaPessoaFisica = _pessoaService.PesquisarPessoaFisica(model.CodigoFuncionario ?? 0, model.Nome,
                    model.CPFCNPJ, model.Email, TipoPapelPessoaEnum.Funcionario);
                    response = ReturnSuccess(Mapper.Map<List<FuncionarioModel>>(listaPessoaFisica));
                }
                else
                {
                    var listaPessoaJuridica = _pessoaService.PesquisarPessoaJuridica(model.CodigoFuncionario ?? 0, model.Nome,
                    model.CPFCNPJ, model.Email, TipoPapelPessoaEnum.Funcionario);
                    response = ReturnSuccess(Mapper.Map<List<FuncionarioModel>>(listaPessoaJuridica));
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

        [Route("PesquisarFuncionarioPorCodigo")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarFuncionarioPorCodigo([FromBody]JObject jObject)
        {
            var codigoFuncionario = jObject["codigoFuncionario"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                var pessoa = _pessoaService.ObterPessoaPorCodigo(codigoFuncionario);
                response =
                    ReturnSuccess(pessoa.PessoaFisica != null
                        ? Mapper.Map<FuncionarioModel>(pessoa.PessoaFisica)
                        : Mapper.Map<FuncionarioModel>(pessoa.PessoaJuridica));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("EditarFuncionario")]
        [HttpPut]
        public Task<HttpResponseMessage> EditarFuncionario(FuncionarioModel model)
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

        [Route("ExcluirFuncionario")]
        [HttpDelete]
        public Task<HttpResponseMessage> ExcluirFuncionario([FromBody]JObject jObject)
        {
            var codigoFuncionario = jObject["codigoFuncionario"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                _pessoaService.ExcluirPessoa(codigoFuncionario);
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
