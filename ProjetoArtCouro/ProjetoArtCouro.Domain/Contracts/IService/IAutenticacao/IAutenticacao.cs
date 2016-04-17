using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.Domain.Contracts.IService.IAutenticacao
{
    public interface IAutenticacao : IDisposable
    {
        Usuario AutenticarUsuario(string usuarioNome, string senha);
        List<Permissao> ObterPermissoes(string usuarioNome);
    }
}
