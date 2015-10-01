using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using ProjetoArtCouro.Domain.Contracts.IService.IUsuario;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Usuario;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Api.Controllers.Usuarios
{
    [RoutePrefix("api/Usuario")]
    public class UsuarioController : ApiController
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [Route("CriarUsuario")]
        [HttpPost]
        public Task<HttpResponseMessage> CriarUsuario(UsuarioModel model)
        {
            HttpResponseMessage response;
            var retornoBase = new RetornoBase<string>()
            {
                Mensagem = Mensagens.ReturnSuccess,
                TemErros = false,
            };

            try
            {
                var permissoes = Mapper.Map<List<Permissao>>(model.Permissoes);
                _usuarioService.Registrar(model.UsuarioNome, model.Senha, model.ConfirmarSenha, permissoes);
                retornoBase.ObjetoRetorno = model.UsuarioNome;
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

        [Route("PesquisarUsuario")]
        [Authorize]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarUsuario(PesquisaUsuarioModel model)
        {
            HttpResponseMessage response;
            try
            {
                var retornoBase = new RetornoBase<List<UsuarioModel>>()
                {
                    Mensagem = Mensagens.ReturnSuccess,
                    TemErros = false
                };
                var listaUsuario = _usuarioService.PesquisarUsuario(model.UsuarioNome, model.PermissaoId, model.Ativo);
                retornoBase.ObjetoRetorno = Mapper.Map<List<UsuarioModel>>(listaUsuario);
                response = Request.CreateResponse(HttpStatusCode.OK, retornoBase);
            }
            catch (Exception ex)
            {
                var retornoBase = new RetornoBase<string>()
                {
                    ObjetoRetorno = ex.Message,
                    Mensagem = Mensagens.ReturnSuccess,
                    TemErros = true,
                };
                response = Request.CreateResponse(HttpStatusCode.BadRequest, retornoBase);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("PesquisarGrupo")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarGrupo(PesquisaGrupoModel model)
        {
            HttpResponseMessage response;
            try
            {
                var retornoBase = new RetornoBase<List<GrupoModel>>()
                {
                    Mensagem = Mensagens.ReturnSuccess,
                    TemErros = false
                };
                var listaGrupo = _usuarioService.PesquisarGrupo(model.GrupoNome, model.GrupoCodigo);
                retornoBase.ObjetoRetorno = Mapper.Map<List<GrupoModel>>(listaGrupo);
                response = Request.CreateResponse(HttpStatusCode.OK, retornoBase);
            }
            catch (Exception ex)
            {
                var retornoBase = new RetornoBase<string>()
                {
                    Mensagem = Mensagens.ReturnSuccess,
                    ObjetoRetorno = ex.Message,
                    TemErros = true
                };
                response = Request.CreateResponse(HttpStatusCode.BadRequest, retornoBase);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("CriarGrupo")]
        [HttpPost]
        public Task<HttpResponseMessage> CriarGrupo(GrupoModel model)
        {
            HttpResponseMessage response;
            var retornoBase = new RetornoBase<string>()
            {
                Mensagem = Mensagens.ReturnSuccess,
                TemErros = false,
            };

            try
            {
                var grupoPermissao = Mapper.Map<GrupoPermissao>(model);
                _usuarioService.CriarGrupoPermissao(grupoPermissao);
                retornoBase.ObjetoRetorno = model.GrupoNome;
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

        [Route("ExcluirGrupo")]
        [HttpGet]
        public Task<HttpResponseMessage> ExcluirGrupo(int codigoGrupo)
        {
            HttpResponseMessage response;
            var retornoBase = new RetornoBase<int?>()
            {
                Mensagem = Mensagens.ReturnSuccess,
                TemErros = false,
            };

            try
            {
                _usuarioService.ExcluirGrupoPermissao(codigoGrupo);
                retornoBase.ObjetoRetorno = codigoGrupo;
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

        [Route("ObterListaPermissao")]
        [HttpGet]
        public Task<HttpResponseMessage> ObterListaPermissao()
        {
            HttpResponseMessage response;
            try
            {
                var listaPermissao = _usuarioService.ObterListaPermissao();
                var retornoBase = new RetornoBase<List<PermissaoModel>>()
                {
                    Mensagem = Mensagens.ReturnSuccess,
                    ObjetoRetorno = Mapper.Map<List<PermissaoModel>>(listaPermissao),
                    TemErros = false
                };
                response = Request.CreateResponse(HttpStatusCode.OK, retornoBase);
            }
            catch (Exception ex)
            {
                var retornoBase = new RetornoBase<string>()
                {
                    Mensagem = Mensagens.ReturnSuccess,
                    ObjetoRetorno =  ex.Message,
                    TemErros = true
                };
                response = Request.CreateResponse(HttpStatusCode.BadRequest, retornoBase);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        protected override void Dispose(bool disposing)
        {
            _usuarioService.Dispose();
        }
    }
}
