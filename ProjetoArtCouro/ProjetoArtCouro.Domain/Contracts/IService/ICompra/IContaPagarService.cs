using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Compras;

namespace ProjetoArtCouro.Domain.Contracts.IService.ICompra
{
    public interface IContaPagarService : IDisposable
    {
        List<ContaPagar> PesquisarContaPagar(int codigoCompra, int codigoFornecedor, DateTime dataEmissao,
            DateTime dataVencimento, int statusContaPagar, string nomeFornecedor, string documento, int codigoUsuario);
        void PagarContas(List<ContaPagar> contasPagar);
    }
}
