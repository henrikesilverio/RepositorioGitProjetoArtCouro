using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario
{
    public interface IGrupoPermissaoRepository : IDisposable
    {
        GrupoPermissao ObterPorId(Guid id);
        GrupoPermissao ObterPorCodigo(int codigo); 
        List<GrupoPermissao> ObterLista();
        void Criar(GrupoPermissao gruposPermissao);
        void Atualizar(GrupoPermissao gruposPermissao);
        void Deletar(GrupoPermissao gruposPermissao);
    }
}
