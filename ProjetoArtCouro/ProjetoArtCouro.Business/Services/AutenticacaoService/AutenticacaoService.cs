using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario;
using ProjetoArtCouro.Domain.Contracts.IService.IAutenticacao;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Resources.Validation;

namespace ProjetoArtCouro.Business.Services.AutenticacaoService
{
    public class AutenticacaoService : IAutenticacao
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AutenticacaoService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public Usuario AutenticarUsuario(string usuarioNome, string senha)
        {
            var usuario = _usuarioRepository.ObterPorUsuarioNome(usuarioNome);
            if (usuario == null || !PasswordAssertionConcern.Encrypt(senha).Equals(usuario.Senha))
            {
                return null;
            }
            return usuario;
        }

        public List<Permissao> ObterPermissoes(string usuarioNome)
        {
            var usuario = _usuarioRepository.ObterComPermissoesComGrupoPorUsuarioNome(usuarioNome);
            var permissoesUsuario = usuario.Permissoes;
            var permissoesGrupo = usuario.GrupoPermissao.Permissoes;
            var permissoes = permissoesGrupo.Union(permissoesUsuario).ToList();
            return permissoes;
        }

        public void Dispose()
        {
            _usuarioRepository.Dispose();
        }
    }
}
