using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Estoques;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IEstoque
{
    public interface IEstoqueRepository : IDisposable
    {
        Estoque ObterPorId(Guid id);
        Estoque ObterPorCodigo(int codigo);
        List<Estoque> ObterLista();
        void Criar(Estoque estoque);
        void Atualizar(Estoque estoque);
        void Deletar(Estoque estoque);
    }
}
