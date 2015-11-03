using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Pagamentos;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IPagamento
{
    public interface IFormaPagamentoRepository : IDisposable
    {
        FormaPagamento ObterPorId(Guid id);
        FormaPagamento ObterPorCodigo(int codigo);
        List<FormaPagamento> ObterLista();
        FormaPagamento Criar(FormaPagamento formaPagamento);
        FormaPagamento Atualizar(FormaPagamento formaPagamento);
        void Deletar(FormaPagamento formaPagamento);
    }
}
