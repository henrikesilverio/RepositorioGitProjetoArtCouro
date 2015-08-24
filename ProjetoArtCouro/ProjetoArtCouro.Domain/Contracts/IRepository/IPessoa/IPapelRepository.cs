using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa
{
    public interface IPapelRepository : IDisposable
    {
        Papel ObterPorId(Guid id);
        Papel ObterPorCodigo(int codigo);
        List<Papel> ObterLista();
        void Criar(Papel papel);
        void Atualizar(Papel papel);
        void Deletar(Papel papel);
    }
}
