using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using ProjetoArtCouro.Domain.Contracts.IService.IUsuario;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Model.Models.Usuarios;

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

            try
            {
                var permissoes = Mapper.Map<List<Permissao>>(model.Permissoes);
                _usuarioService.Registrar(model.UsuarioNome, model.Senha, model.ConfirmarSenha, permissoes);
                response = Request.CreateResponse(HttpStatusCode.OK, new {nome = model.UsuarioNome});
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
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
