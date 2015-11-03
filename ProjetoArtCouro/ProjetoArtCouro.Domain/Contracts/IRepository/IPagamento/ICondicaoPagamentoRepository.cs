using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Pagamentos;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IPagamento
{
    public interface ICondicaoPagamentoRepository : IDisposable
    {
        CondicaoPagamento ObterPorId(Guid id);
        CondicaoPagamento ObterPorCodigo(int codigo);
        List<CondicaoPagamento> ObterLista();
        CondicaoPagamento Criar(CondicaoPagamento condicaoPagamento);
        CondicaoPagamento Atualizar(CondicaoPagamento condicaoPagamento);
        void Deletar(CondicaoPagamento condicaoPagamento);
    }
}
