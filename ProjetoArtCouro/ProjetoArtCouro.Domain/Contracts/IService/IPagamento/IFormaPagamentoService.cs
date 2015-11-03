using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Pagamentos;

namespace ProjetoArtCouro.Domain.Contracts.IService.IPagamento
{
    public interface IFormaPagamentoService : IDisposable
    {
        List<FormaPagamento> ObterListaFormaPagamento();
        FormaPagamento ObterFormaPagamentoPorCodigo(int codigo);
        FormaPagamento CriarFormaPagamento(FormaPagamento formaPagamento);
        FormaPagamento AtualizarFormaPagamento(FormaPagamento formaPagamento);
        void ExcluirFormaPagamento(int formaPagamentoCodigo);
    }
}
