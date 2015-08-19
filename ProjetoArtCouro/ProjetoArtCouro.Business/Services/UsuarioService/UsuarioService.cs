using System;
using System.Collections.Generic;
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
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public void Registrar(string nome, string senha, string confimaSenha, List<Permissao> permissoes)
        {
            AssertionConcern.AssertArgumentNotEmpty(nome, Erros.InvalidUserName);
            var temUsuario = _usuarioRepository.ObterPorUsuarioNome(nome.ToLower());
            if (temUsuario != null)
            {
                throw new Exception(Erros.DuplicateName);
            };

            var usuario = new Usuario()
            {
                UsuarioNome = nome.ToLower(),
                Senha = PasswordAssertionConcern.Encrypt(senha),
                Permissoes = permissoes
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

        public void Dispose()
        {
            _usuarioRepository.Dispose();
        }
    }
}
