using System;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.Domain.Contracts.IService.IAutenticacao
{
    public interface IAutenticacao : IDisposable
    {
        Usuario AutenticarUsuario(string usuarioNome, string senha);
    }
}
