using System;
using System.Collections.Generic;
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
            var retornoBase = new RetornoBase<string>()
            {
                Mensagem = Mensagens.ReturnSuccess,
                TemErros = false,
            };

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
                //var pessoa = new Pessoa{Nome = model.Nome };
                //var enderero = new Endereco
                //{
                //    Bairro = model.Endereco.Bairro,
                //    CEP = model.Endereco.Cep,
                //    Cidade = model.Endereco.Cidade,
                //    Logradouro = model.Endereco.Logradouro,
                //    Complemento = model.Endereco.Complemento,
                //    Numero = model.Endereco.Numero,
                //    Principal = true,
                //    Estado = new Estado { EstadoCodigo = model.Endereco.UFId ?? 0 }
                //};
                //var meioComunicacao = new MeioComunicacao
                //{
                //    MeioComunicacaoNome = model.MeioComunicacao.Telefone,
                //    Principal = true,
                //    TipoComunicacao = TipoComunicacaoEnum.Telefone
                //};
                //var pessoaFisica = new PessoaFisica
                //{
                //    CPF = model.CPF, 
                //    RG = model.RG, 
                //    Sexo = model.Sexo,
                //    EstadoCivil = new EstadoCivil {EstadoCivilCodigo = model.EstadoCivilId ?? 0}
                //};
                //pessoa.Enderecos = new List<Endereco> { enderero };
                //pessoa.MeiosComunicacao = new List<MeioComunicacao> {meioComunicacao};
                //pessoa.Papeis = new List<Papel> { new Papel { PapelCodigo = model.PapelPessoa } };
                //pessoa.PessoaFisica = pessoaFisica;

                _pessoaService.CriarPessoaFisica(pessoa);
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
            var retornoBase = new RetornoBase<List<ClienteModel>>()
            {
                Mensagem = Mensagens.ReturnSuccess,
                TemErros = false,
            };

            try
            {
                var listaPessoaFisica = _pessoaService.PesquisarPessoaFisica(model.CodigoCliente ?? 0, model.Nome,
                    model.CPFCNPJ, model.Email);
                retornoBase.ObjetoRetorno = Mapper.Map<List<ClienteModel>>(listaPessoaFisica);
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
