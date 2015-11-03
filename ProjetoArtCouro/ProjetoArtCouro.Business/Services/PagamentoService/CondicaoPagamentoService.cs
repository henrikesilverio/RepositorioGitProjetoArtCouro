using System.Collections.Generic;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPagamento;
using ProjetoArtCouro.Domain.Contracts.IService.IPagamento;
using ProjetoArtCouro.Domain.Models.Pagamentos;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Business.Services.PagamentoService
{
    public class CondicaoPagamentoService : ICondicaoPagamentoService
    {
        private readonly ICondicaoPagamentoRepository _condicaoPagamentoRepository; 
        public CondicaoPagamentoService(ICondicaoPagamentoRepository condicaoPagamentoRepository)
        {
            _condicaoPagamentoRepository = condicaoPagamentoRepository;
        }

        public List<CondicaoPagamento> ObterListaCondicaoPagamento()
        {
            return _condicaoPagamentoRepository.ObterLista();
        }

        public CondicaoPagamento ObterCondicaoPagamentoPorCodigo(int codigo)
        {
            return _condicaoPagamentoRepository.ObterPorCodigo(codigo);
        }

        public CondicaoPagamento CriarCondicaoPagamento(CondicaoPagamento condicaoPagamento)
        {
            condicaoPagamento.Validar();
            return _condicaoPagamentoRepository.Criar(condicaoPagamento);
        }

        public CondicaoPagamento AtualizarCondicaoPagamento(CondicaoPagamento condicaoPagamento)
        {
            condicaoPagamento.Validar();
            AssertionConcern.AssertArgumentNotEquals(0, condicaoPagamento.CondicaoPagamentoCodigo,
                string.Format(Erros.NotZeroParameter, "CondicaoPagamentoCodigo"));
            var condicaoPagamentoAtual =
                _condicaoPagamentoRepository.ObterPorCodigo(condicaoPagamento.CondicaoPagamentoCodigo);
            condicaoPagamentoAtual.Ativo = condicaoPagamento.Ativo;
            condicaoPagamentoAtual.Descricao = condicaoPagamento.Descricao;
            condicaoPagamentoAtual.QuantidadeParcelas = condicaoPagamento.QuantidadeParcelas;
            return _condicaoPagamentoRepository.Atualizar(condicaoPagamentoAtual);
        }

        public void ExcluirCondicaoPagamento(int condicaoPagamentoCodigo)
        {
            var condicaoPagamentoAtual = _condicaoPagamentoRepository.ObterPorCodigo(condicaoPagamentoCodigo);
            AssertionConcern.AssertArgumentNotEquals(condicaoPagamentoAtual, null, Erros.PaymentConditionDoesNotExist);
            _condicaoPagamentoRepository.Deletar(condicaoPagamentoAtual);
        }

        public void Dispose()
        {
            _condicaoPagamentoRepository.Dispose();
        }
    }
}
