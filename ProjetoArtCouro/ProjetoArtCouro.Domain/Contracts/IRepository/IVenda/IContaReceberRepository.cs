using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Vendas;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IVenda
{
    public interface IContaReceberRepository : IDisposable
    {
        ContaReceber ObterPorId(Guid id);
        ContaReceber ObterPorCodigo(int codigo);
        ContaReceber ObterPorCodigoComVenda(int codigo);
        List<ContaReceber> ObterLista();
        List<ContaReceber> ObterListaPorCodigoVenda(int codigoVenda);
        List<ContaReceber> ObterLista(int codigoVenda, int codigoCliente, DateTime dataEmissao, DateTime dataVencimento, int statusContaReceber,
            string nomeCliente, string documento, int codigoUsuario);
        void Criar(ContaReceber contaReceber);
        void Atualizar(ContaReceber contaReceber);
        void Deletar(ContaReceber contaReceber);
    }
}
