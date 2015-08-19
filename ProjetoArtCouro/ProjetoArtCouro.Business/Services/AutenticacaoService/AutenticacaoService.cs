using ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario;
using ProjetoArtCouro.Domain.Contracts.IService.IAutenticacao;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Resource.Validation;

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
            var usuario = _usuarioRepository.ObterComPermissoesPorUsuarioNome(usuarioNome);
            if (usuario == null || !PasswordAssertionConcern.Encrypt(senha).Equals(usuario.Senha))
            {
                return null;
            }
            return usuario;
        }
    }
}
