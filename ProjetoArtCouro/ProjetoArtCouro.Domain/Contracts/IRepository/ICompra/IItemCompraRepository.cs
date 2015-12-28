using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Compras;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.ICompra
{
    public interface IItemCompraRepository : IDisposable
    {
        ItemCompra ObterPorId(Guid id);
        ItemCompra ObterPorCodigo(int codigo);
        List<ItemCompra> ObterLista();
        ItemCompra Criar(ItemCompra itemCompra);
        void Atualizar(ItemCompra itemCompra);
        void Deletar(ItemCompra itemCompra);
    }
}
