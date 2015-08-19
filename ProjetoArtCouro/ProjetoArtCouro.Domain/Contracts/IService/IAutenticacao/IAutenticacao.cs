using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.Domain.Contracts.IService.IAutenticacao
{
    public interface IAutenticacao
    {
        Usuario AutenticarUsuario(string usuarioNome, string senha);
    }
}
