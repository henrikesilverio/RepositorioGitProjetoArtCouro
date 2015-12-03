using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario
{
    public interface IGrupoPermissaoRepository : IDisposable
    {
        GrupoPermissao ObterPorId(Guid id);
        GrupoPermissao ObterPorCodigo(int codigo);
        GrupoPermissao ObterPorCodigoComPermissoes(int codigo);
        GrupoPermissao ObterPorCodigoComPermissoesEUsuarios(int codigo);
        GrupoPermissao ObterPorGrupoPermissaoNome(string nome);
        List<GrupoPermissao> ObterLista();
        List<GrupoPermissao> ObterLista(string nome, int? codigo);
        void Criar(GrupoPermissao gruposPermissao);
        void Atualizar(GrupoPermissao gruposPermissao);
        void Deletar(GrupoPermissao gruposPermissao);
    }
}
