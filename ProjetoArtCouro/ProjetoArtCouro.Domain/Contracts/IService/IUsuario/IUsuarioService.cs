using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.Domain.Contracts.IService.IUsuario
{
    public interface IUsuarioService : IDisposable
    {
        void Registrar(string nome, string senha, string confirmaSenha, List<Permissao> permissoes);
        void AlterarSenha(Usuario usuario, string senha, string novaSenha, string confirmaNovaSenha);
        List<Usuario> ObterLista();
    }
}
