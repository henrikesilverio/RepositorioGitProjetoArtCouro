using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario
{
    public interface IUsuarioRepository : IDisposable
    {
        Usuario ObterPorId(Guid id);
        Usuario ObterPorUsuarioNome(string usuarioNome);
        Usuario ObterComPermissoesPorUsuarioNome(string usuarioNome);
        List<Usuario> ObterLista();
        List<Usuario> ObterLista(string nome, int? permissaoId, bool? ativo);
        void Criar(Usuario usuario);
        void Atualizar(Usuario usuario);
        void Deletar(Usuario usuario);
    }
}
