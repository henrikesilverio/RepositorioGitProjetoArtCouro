using System.Collections.Generic;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPagamento;
using ProjetoArtCouro.Domain.Contracts.IService.IPagamento;
using ProjetoArtCouro.Domain.Models.Pagamentos;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Business.Services.PagamentoService
{
    public class FormaPagamentoService : IFormaPagamentoService
    {
        private readonly IFormaPagamentoRepository _formaPagamentoRepository;
        public FormaPagamentoService(IFormaPagamentoRepository formaPagamentoRepository)
        {
            _formaPagamentoRepository = formaPagamentoRepository;
        }

        public List<FormaPagamento> ObterListaFormaPagamento()
        {
            return _formaPagamentoRepository.ObterLista();
        }

        public FormaPagamento ObterFormaPagamentoPorCodigo(int codigo)
        {
            return _formaPagamentoRepository.ObterPorCodigo(codigo);
        }

        public FormaPagamento CriarFormaPagamento(FormaPagamento formaPagamento)
        {
            formaPagamento.Validar();
            return _formaPagamentoRepository.Criar(formaPagamento);
        }

        public FormaPagamento AtualizarFormaPagamento(FormaPagamento formaPagamento)
        {
            formaPagamento.Validar();
            AssertionConcern.AssertArgumentNotEquals(0, formaPagamento.FormaPagamentoCodigo,
                string.Format(Erros.NotZeroParameter, "FormaPagamentoCodigo"));
            var formaPagamentoAtual =
                _formaPagamentoRepository.ObterPorCodigo(formaPagamento.FormaPagamentoCodigo);
            formaPagamentoAtual.Ativo = formaPagamento.Ativo;
            formaPagamentoAtual.Descricao = formaPagamento.Descricao;
            return _formaPagamentoRepository.Atualizar(formaPagamentoAtual);
        }

        public void ExcluirFormaPagamento(int formaPagamentoCodigo)
        {
            var formaPagamentoAtual = _formaPagamentoRepository.ObterPorCodigo(formaPagamentoCodigo);
            AssertionConcern.AssertArgumentNotEquals(formaPagamentoAtual, null, Erros.FormOfPaymentDoesNotExist);
            _formaPagamentoRepository.Deletar(formaPagamentoAtual);
        }

        public void Dispose()
        {
            _formaPagamentoRepository.Dispose();
        }
    }
}
