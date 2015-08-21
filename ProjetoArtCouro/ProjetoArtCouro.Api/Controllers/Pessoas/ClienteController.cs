using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using ProjetoArtCouro.Domain.Contracts.IService.IPessoa;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Model.Models.Cliente;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Api.Controllers.Pessoas
{
    [RoutePrefix("api/Cliente")]
    public class ClienteController : ApiController
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
            var retornoBase = new RetornoBase()
            {
                Mensagem = Mensagens.ReturnSuccess,
                TemErros = false,
            };

            try
            {
                //Mapeamento
                var pessoaFisica = Mapper.Map<PessoaFisica>(model);
                pessoaFisica.Pessoa.Papeis.Add(new Papel { Codigo = model.PapelPessoa });
                _pessoaService.CriarPessoaFisica(pessoaFisica);
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

        [Route("PesquisarCliente")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarCliente(PesquisaClienteModel model)
        {
            HttpResponseMessage response;
            var retornoBase = new RetornoBase()
            {
                Mensagem = Mensagens.ReturnSuccess,
                TemErros = false,
            };

            try
            {
                retornoBase.ObjetoRetorno = _pessoaService.PesquisarPessoaFisica(model.CodigoCliente ?? 0, model.Nome,
                    model.CPFCNPJ, model.Email);
                response = Request.CreateResponse(HttpStatusCode.OK, retornoBase);
            }
            catch (Exception ex)
            {
                retornoBase.ObjetoRetorno = null;
                retornoBase.TemErros = true;
                retornoBase.Mensagem = ex.Message;
                response = Request.CreateResponse(HttpStatusCode.BadRequest, retornoBase);
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
            //    response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
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
            //    response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
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
