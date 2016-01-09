using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Compras;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.ICompra
{
    public interface IContaPagarRepository : IDisposable
    {
        ContaPagar ObterPorId(Guid id);
        ContaPagar ObterPorCodigo(int codigo);
        ContaPagar ObterPorCodigoComCompra(int codigo);
        List<ContaPagar> ObterLista();
        List<ContaPagar> ObterListaPorCodigoCompra(int codigoCompra);
        List<ContaPagar> ObterLista(int codigoCompra, int codigoFornecedor, DateTime dataEmissao, DateTime dataVencimento, int statusContaPagar,
            string nomeFornecedor, string documento, int codigoUsuario);
        void Criar(ContaPagar contaPagar);
        void Atualizar(ContaPagar contaPagar);
        void Deletar(ContaPagar contaPagar);
    }
}
