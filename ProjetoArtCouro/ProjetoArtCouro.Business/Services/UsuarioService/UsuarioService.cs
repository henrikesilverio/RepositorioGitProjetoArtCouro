using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario;
using ProjetoArtCouro.Domain.Contracts.IService.IUsuario;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Business.Services.UsuarioService
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPermissaoRepository _permissaoRepository;
        private readonly IGrupoPermissaoRepository _grupoPermissaoRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository, IPermissaoRepository permissaoRepository,
            IGrupoPermissaoRepository grupoPermissaoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _permissaoRepository = permissaoRepository;
            _grupoPermissaoRepository = grupoPermissaoRepository;
        }

        public void Registrar(string nome, string senha, string confimaSenha, List<Permissao> permissoes)
        {
            AssertionConcern.AssertArgumentNotEmpty(nome, Erros.InvalidUserName);
            var temUsuario = _usuarioRepository.ObterPorUsuarioNome(nome.ToLower());
            if (temUsuario != null)
            {
                throw new Exception(Erros.DuplicateName);
            };

            var listaPermissao =
                permissoes.Select(x => _permissaoRepository.ObterPermissaoPorCodigo(x.PermissaoCodigo)).ToList();
            if (!listaPermissao.Any())
            {
                throw new Exception(Erros.PermissionsNotRegistered);
            }

            var usuario = new Usuario()
            {
                UsuarioNome = nome.ToLower(),
                Senha = PasswordAssertionConcern.Encrypt(senha),
                Permissoes = listaPermissao
            };

            usuario.Validar();
            _usuarioRepository.Criar(usuario);
        }

        public void AlterarSenha(Usuario usuario, string senha, string novaSenha, string confirmaNovaSenha)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> ObterLista()
        {
            throw new NotImplementedException();
        }

        public List<Permissao> ObterListaPermissao()
        {
            return _permissaoRepository.ObterLista();
        }

        public List<Usuario> PesquisarUsuario(string nome, int? permissaoId, bool? ativo)
        {
            return _usuarioRepository.ObterLista(nome, permissaoId, ativo);
        }

        public void Dispose()
        {
            _usuarioRepository.Dispose();
        }
    }
}
