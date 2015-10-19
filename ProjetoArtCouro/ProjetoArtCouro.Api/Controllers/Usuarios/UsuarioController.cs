using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Newtonsoft.Json.Linq;
using ProjetoArtCouro.Api.Extensions;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Domain.Contracts.IService.IUsuario;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Model.Models.Usuario;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Api.Controllers.Usuarios
{
    [RoutePrefix("api/Usuario")]
    [DeflateCompression]
    public class UsuarioController : ApiControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [Route("CriarUsuario")]
        [Authorize(Roles = "NovoUsuario")]
        [HttpPost]
        public Task<HttpResponseMessage> CriarUsuario(UsuarioModel model)
        {
            HttpResponseMessage response;
            try
            {
                if (model.GrupoId == null)
                {
                    throw new Exception(string.Format(Erros.NullParameter, "GrupoId"));
                }
                _usuarioService.Registrar(model.UsuarioNome, model.Senha, model.ConfirmarSenha, model.GrupoId.Value);
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

        [Route("EditarUsuario")]
        [Authorize(Roles = "EditarUsuario")]
        [HttpPut]
        public Task<HttpResponseMessage> EditarUsuario(UsuarioModel model)
        {
            HttpResponseMessage response;
            try
            {
                var usuario = Mapper.Map<Usuario>(model);
                _usuarioService.EditarUsuario(usuario);
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

        [Route("AlterarSenha")]
        [Authorize(Roles = "AlterarSenha")]
        [HttpPut]
        public Task<HttpResponseMessage> AlterarSenha(UsuarioModel model)
        {
            HttpResponseMessage response;
            try
            {
                var identity = (ClaimsPrincipal) Thread.CurrentPrincipal;
                var usuarioNome = identity.Claims.Where(c => c.Type == ClaimTypes.GivenName)
                    .Select(c => c.Value).SingleOrDefault();
                var usuario = new Usuario
                {
                    UsuarioNome = usuarioNome,
                    Senha = model.Senha
                };
                _usuarioService.AlterarSenha(usuario);
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

        [Route("ObterPermissoesUsuarioLogado")]
        [Authorize]
        [HttpGet]
        public Task<HttpResponseMessage> ObterPermissoesUsuarioLogado()
        {
            HttpResponseMessage response;
            try
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var usuarioNome = identity.Claims.Where(c => c.Type == ClaimTypes.GivenName)
                    .Select(c => c.Value).SingleOrDefault();
                var listaPermissao = _usuarioService.ObterPermissoesUsuarioLogado(usuarioNome);
                response = ReturnSuccess(Mapper.Map<List<PermissaoModel>>(listaPermissao));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("ExcluirUsuario")]
        [Authorize(Roles = "ExcluirUsuario")]
        [HttpDelete]
        public Task<HttpResponseMessage> ExcluirUsuario([FromBody]JObject jObject)
        {
            var codigoUsuario = jObject["codigoUsuario"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                _usuarioService.ExcluirUsuario(codigoUsuario);
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

        [Route("PesquisarUsuario")]
        [Authorize(Roles = "PesquisaUsuario")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarUsuario(PesquisaUsuarioModel model)
        {
            HttpResponseMessage response;
            try
            {
                var listaUsuario = _usuarioService.PesquisarUsuario(model.UsuarioNome, model.GrupoId, model.Ativo);
                response = ReturnSuccess(Mapper.Map<List<UsuarioModel>>(listaUsuario));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("PesquisarUsuarioPorCodigo")]
        [Authorize(Roles = "EditarUsuario")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarUsuarioPorCodigo([FromBody]JObject jObject)
        {
            var codigoUsuario = jObject["codigoUsuario"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                var usuario = _usuarioService.PesquisarUsuarioPorCodigo(codigoUsuario);
                response = ReturnSuccess(Mapper.Map<UsuarioModel>(usuario));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("ObterListaUsuario")]
        [Authorize(Roles = "ConfiguracaoUsuario")]
        [HttpGet]
        public Task<HttpResponseMessage> ObterListaUsuario()
        {
            HttpResponseMessage response;
            try
            {
                var listaUsuario = _usuarioService.ObterListaUsuario();
                response = ReturnSuccess(Mapper.Map<List<UsuarioModel>>(listaUsuario));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("EditarPermissaoUsuario")]
        [Authorize(Roles = "ConfiguracaoUsuario")]
        [HttpPut]
        public Task<HttpResponseMessage> EditarPermissaoUsuario(ConfiguracaoUsuarioModel model)
        {
            HttpResponseMessage response;
            try
            {
                if (!model.UsuarioId.HasValue)
                {
                    throw new Exception(string.Format(Erros.NullParameter, "UsuarioId"));
                }
                _usuarioService.EditarPermissaoUsuario(model.UsuarioId.Value,
                    Mapper.Map<List<Permissao>>(model.Permissoes));
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

        [Route("PesquisarGrupo")]
        [Authorize(Roles = "PesquisaGrupo")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarGrupo(PesquisaGrupoModel model)
        {
            HttpResponseMessage response;
            try
            {
                var listaGrupo = _usuarioService.PesquisarGrupo(model.GrupoNome, model.GrupoCodigo, model.Todos);
                response = ReturnSuccess(Mapper.Map<List<GrupoModel>>(listaGrupo));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("PesquisarGrupoPorCodigo")]
        [Authorize(Roles = "EditarGrupo")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarGrupoPorCodigo([FromBody]JObject jObject)
        {
            var codigoGrupo = jObject["codigoGrupo"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                var grupoPermissao = _usuarioService.ObterGrupoPermissaoPorCodigo(codigoGrupo);
                response = ReturnSuccess(Mapper.Map<GrupoModel>(grupoPermissao));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("CriarGrupo")]
        [Authorize(Roles = "NovoGrupo")]
        [HttpPost]
        public Task<HttpResponseMessage> CriarGrupo(GrupoModel model)
        {
            HttpResponseMessage response;
            try
            {
                var grupoPermissao = Mapper.Map<GrupoPermissao>(model);
                _usuarioService.CriarGrupoPermissao(grupoPermissao);
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

        [Route("EditarGrupo")]
        [Authorize(Roles = "EditarGrupo")]
        [HttpPut]
        public Task<HttpResponseMessage> EditarGrupo(GrupoModel model)
        {
            HttpResponseMessage response;
            try
            {
                var grupoPermissao = Mapper.Map<GrupoPermissao>(model);
                _usuarioService.EditarGrupoPermissao(grupoPermissao);
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

        [Route("ExcluirGrupo")]
        [Authorize(Roles = "ExcluirGrupo")]
        [HttpDelete]
        public Task<HttpResponseMessage> ExcluirGrupo([FromBody]JObject jObject)
        {
            var codigoGrupo = jObject["codigoGrupo"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                _usuarioService.ExcluirGrupoPermissao(codigoGrupo);
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

        [Route("ObterListaGrupo")]
        [Authorize(Roles = "PesquisaUsuario, NovoUsuario, EditarUsuario")]
        [HttpGet]
        public Task<HttpResponseMessage> ObterListaGrupo()
        {
            HttpResponseMessage response;
            try
            {
                var listaGrupo = _usuarioService.ObterListaGrupoPermissao();
                response = ReturnSuccess(Mapper.Map<List<GrupoModel>>(listaGrupo));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("ObterListaPermissao")]
        [Authorize(Roles = "NovoGrupo, EditarGrupo, ConfiguracaoUsuario")]
        [HttpGet]
        public Task<HttpResponseMessage> ObterListaPermissao()
        {
            HttpResponseMessage response;
            try
            {
                var listaPermissao = _usuarioService.ObterListaPermissao();
                response = ReturnSuccess(Mapper.Map<List<PermissaoModel>>(listaPermissao));
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
            _usuarioService.Dispose();
        }
    }
}
