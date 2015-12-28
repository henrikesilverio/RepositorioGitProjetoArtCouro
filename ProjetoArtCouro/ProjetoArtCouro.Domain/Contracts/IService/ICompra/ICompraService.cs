using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Compras;

namespace ProjetoArtCouro.Domain.Contracts.IService.ICompra
{
    public interface ICompraService : IDisposable
    {
        List<Compra> PesquisarCompra(int codigoCompra, int codigoFornecedor, DateTime dataCadastro,
        int statusCompra, string nomeFornecedor, string documento, int codigoUsuario);
        Compra ObterCompraPorCodigo(int codigoCompra);
        void CriarCompra(Compra compra);
        void AtualizarCompra(Compra compra);
        void ExcluirCompra(int codigoCompra);
    }
}
