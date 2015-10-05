using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Domain.Contracts.IService.IPessoa;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Model.Models.Cliente;

namespace ProjetoArtCouro.Api.Controllers.Pessoas
{
    [RoutePrefix("api/Cliente")]
    public class ClienteController : ApiControllerBase
    {
        private readonly IPessoaService _pessoaService;
        public ClienteController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [Route("CriarCliente")]
        [HttpPost]
        public Task<HttpResponseMessage> CriarCliente(ClienteModel model)
        {
            HttpResponseMessage response;
            try
            {
                var pessoa = Mapper.Map<Pessoa>(model);
                if (model.EPessoaFisica)
                {
                    pessoa.PessoaFisica = Mapper.Map<PessoaFisica>(model);
                }
                else
                {
                    pessoa.PessoaJuridica = Mapper.Map<PessoaJuridica>(model);
                }
                pessoa.Papeis = new List<Papel> { new Papel { PapelCodigo = model.PapelPessoa } };
                //Remove informações que não vão ser gravadas.
                ((List<MeioComunicacao>)pessoa.MeiosComunicacao).RemoveAll(
                    x => string.IsNullOrEmpty(x.MeioComunicacaoNome) && x.MeioComunicacaoCodigo.Equals(0));
                _pessoaService.CriarPessoaFisica(pessoa);
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

        [Route("PesquisarCliente")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarCliente(PesquisaClienteModel model)
        {
            HttpResponseMessage response;
            try
            {
                var listaPessoaFisica = _pessoaService.PesquisarPessoaFisica(model.CodigoCliente ?? 0, model.Nome,
                    model.CPFCNPJ, model.Email);
                response = ReturnSuccess(Mapper.Map<List<ClienteModel>>(listaPessoaFisica));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("AtualizarCliente")]
        [HttpPost]
        public Task<HttpResponseMessage> AtualizarCliente(ClienteModel model)
        {
            HttpResponseMessage response;

            //try
            //{
            //    var permissoes = Mapper.Map<List<Permissao>>(model.Permissoes);
            //    _pessoaService.Registrar(model.UsuarioNome, model.Senha, model.ConfirmarSenha, permissoes);
            //    response = Request.CreateResponse(HttpStatusCode.OK, new { nome = model.UsuarioNome });
            //}
            //catch (Exception ex)
            //{
            //    response = ReturnError(ex);
            //}

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            //tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("DeletarCliente")]
        [HttpPost]
        public Task<HttpResponseMessage> DeletarCliente(ClienteModel model)
        {
            HttpResponseMessage response;

            //try
            //{
            //    var permissoes = Mapper.Map<List<Permissao>>(model.Permissoes);
            //    _pessoaService.Registrar(model.UsuarioNome, model.Senha, model.ConfirmarSenha, permissoes);
            //    response = Request.CreateResponse(HttpStatusCode.OK, new { nome = model.UsuarioNome });
            //}
            //catch (Exception ex)
            //{
            //    response = ReturnError(ex);
            //}

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            //tsc.SetResult(response);
            return tsc.Task;
        }

        protected override void Dispose(bool disposing)
        {
            _pessoaService.Dispose();
        }
    }
}
