using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Compras;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.ICompra
{
    public interface ICompraRepository : IDisposable
    {
        Compra ObterPorId(Guid id);
        Compra ObterPorCodigo(int codigo);
        Compra ObterPorCodigoComItensCompra(int codigo);
        List<Compra> ObterLista();
        List<Compra> ObterLista(int codigoCompra, int codigoFornecedor, DateTime dataCadastro, int statusCompra,
            string nomeFornecedor, string documento, int codigoUsuario);
        void Criar(Compra compra);
        void Atualizar(Compra compra);
        void Deletar(Compra compra);
    }
}
