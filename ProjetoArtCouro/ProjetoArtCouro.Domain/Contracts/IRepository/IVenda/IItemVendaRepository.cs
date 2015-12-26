using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Vendas;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IVenda
{
    public interface IItemVendaRepository : IDisposable
    {
        ItemVenda ObterPorId(Guid id);
        ItemVenda ObterPorCodigo(int codigo);
        List<ItemVenda> ObterLista();
        ItemVenda Criar(ItemVenda itemVenda);
        void Atualizar(ItemVenda itemVenda);
        void Deletar(ItemVenda itemVenda);
    }
}
