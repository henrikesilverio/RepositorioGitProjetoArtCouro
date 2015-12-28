using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using ProjetoArtCouro.Domain.Contracts.IRepository.ICompra;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPagamento;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
using ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario;
using ProjetoArtCouro.Domain.Contracts.IService.ICompra;
using ProjetoArtCouro.Domain.Models.Compras;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Business.Services.CompraService
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IItemCompraRepository _itemCompraRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IFormaPagamentoRepository _formaPagamentoRepository;
        private readonly ICondicaoPagamentoRepository _condicaoPagamentoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public CompraService(ICompraRepository vendaRepository, IItemCompraRepository itemVendaRepository,
            IPessoaRepository pessoaRepository, IFormaPagamentoRepository formaPagamentoRepository,
            ICondicaoPagamentoRepository condicaoPagamentoRepository, IUsuarioRepository usuarioRepository)
        {
            _compraRepository = vendaRepository;
            _itemCompraRepository = itemVendaRepository;
            _pessoaRepository = pessoaRepository;
            _formaPagamentoRepository = formaPagamentoRepository;
            _condicaoPagamentoRepository = condicaoPagamentoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public List<Compra> PesquisarCompra(int codigoCompra, int codigoFornecedor, DateTime dataCadastro, int statusCompra,
            string nomeFornecedor, string documento, int codigoUsuario)
        {
            if (codigoCompra.Equals(0) && codigoFornecedor.Equals(0) &&
                dataCadastro.Equals(new DateTime()) && statusCompra.Equals(0) &&
                string.IsNullOrEmpty(nomeFornecedor) && string.IsNullOrEmpty(documento) &&
                codigoUsuario.Equals(0))
            {
                throw new Exception(Erros.EmptyParameters);
            };

            return _compraRepository.ObterLista(codigoCompra, codigoFornecedor, dataCadastro, statusCompra, nomeFornecedor,
                documento, codigoUsuario);
        }

        public Compra ObterCompraPorCodigo(int codigoCompra)
        {
            return _compraRepository.ObterPorCodigoComItensCompra(codigoCompra);
        }

        public void CriarCompra(Compra compra)
        {
            compra.Validar();
            compra.ItensCompra.ForEach(x => x.Validar());
            AssertionConcern.AssertArgumentEquals(compra.StatusCompra, StatusCompraEnum.Aberto, Erros.StatusOfDifferentPurchasingOpen);
            AplicaValidacoesPadrao(compra);
            var usuario = _usuarioRepository.ObterPorCodigo(compra.Usuario.UsuarioCodigo);
            compra.Usuario = usuario;
            compra.Fornecedor = null;
            compra.FormaPagamento = null;
            compra.CondicaoPagamento = null;
            _compraRepository.Criar(compra);
        }

        public void AtualizarCompra(Compra compra)
        {
            compra.Validar();
            compra.ItensCompra.ForEach(x => x.Validar());
            AssertionConcern.AssertArgumentNotEquals(0, compra.CompraCodigo,
                string.Format(Erros.NotZeroParameter, "CompraCodigo"));
            var compraAtual = _compraRepository.ObterPorCodigoComItensCompra(compra.CompraCodigo);
            if (compra.StatusCompra == StatusCompraEnum.Aberto)
            {
                AplicaValidacoesPadrao(compra);
                AdicionaFornecedorFormaECondicaoDePagamento(compra, compraAtual);
                AtualizaItensCompra(compraAtual, compra.ItensCompra);
                compraAtual.StatusCompra = StatusCompraEnum.Confirmado;
                //TODO Gera conta a pagar
                //TODO Adiciona no estoque
            }
            else
            {
                compraAtual.StatusCompra = StatusCompraEnum.Cancelado;
                //TODO Remove o conta a pagar
                //TODO Remove do estoque
            }
            _compraRepository.Atualizar(compraAtual);
        }

        public void ExcluirCompra(int codigoCompra)
        {
            var compra = _compraRepository.ObterPorCodigo(codigoCompra);
            AssertionConcern.AssertArgumentNotEquals(compra, null, Erros.PurchaseDoesNotExist);
            AssertionConcern.AssertArgumentNotEquals(compra.StatusCompra, StatusVendaEnum.Confirmado, Erros.PurchaseConfirmedCanNotBeExcluded);
            _compraRepository.Deletar(compra);
        }

        private void AtualizaItensCompra(Compra compraAtual, IEnumerable<ItemCompra> itensCompra)
        {
            var itensCompraAtual = compraAtual.ItensCompra.ToList();
            itensCompraAtual.ForEach(x =>
            {
                _itemCompraRepository.Deletar(x);
            });
            compraAtual.ItensCompra.Clear();
            itensCompra.ForEach(x =>
            {
                x.Compra = compraAtual;
                var novoItem = _itemCompraRepository.Criar(x);
                compraAtual.ItensCompra.Add(novoItem);
            });
        }

        private static void AplicaValidacoesPadrao(Compra compra)
        {
            AssertionConcern.AssertArgumentEquals(compra.ItensCompra.Sum(x => x.ValorBruto), compra.ValorTotalBruto,
                    Erros.SumDoNotMatchTotalCrudeValue);
            AssertionConcern.AssertArgumentEquals(compra.ItensCompra.Sum(x => x.ValorFrete),
                compra.ValorTotalFrete,
                Erros.SumDoNotMatchTotalValueShipping);
            AssertionConcern.AssertArgumentEquals(compra.ItensCompra.Sum(x => x.ValorLiquido), compra.ValorTotalLiquido,
                Erros.SumDoNotMatchTotalValueLiquid);
        }

        private void AdicionaFornecedorFormaECondicaoDePagamento(Compra compra, Compra compraAtual)
        {
            AssertionConcern.AssertArgumentNotEquals(0, compra.Fornecedor.PessoaCodigo, Erros.ProviderNotSet);
            AssertionConcern.AssertArgumentNotEquals(0, compra.FormaPagamento.FormaPagamentoCodigo,
                Erros.NotSetPayment);
            AssertionConcern.AssertArgumentNotEquals(0, compra.CondicaoPagamento.CondicaoPagamentoCodigo,
                Erros.PaymentConditionNotSet);
            var fornecedor = _pessoaRepository.ObterPorCodigo(compra.Fornecedor.PessoaCodigo);
            var formaPagamento = _formaPagamentoRepository.ObterPorCodigo(compra.FormaPagamento.FormaPagamentoCodigo);
            var condicaoPagamento =
                _condicaoPagamentoRepository.ObterPorCodigo(compra.CondicaoPagamento.CondicaoPagamentoCodigo);
            compraAtual.Fornecedor = fornecedor;
            compraAtual.FormaPagamento = formaPagamento;
            compraAtual.CondicaoPagamento = condicaoPagamento;
        }

        public void Dispose()
        {
            _compraRepository.Dispose();
            _itemCompraRepository.Dispose();
            _pessoaRepository.Dispose();
            _formaPagamentoRepository.Dispose();
            _condicaoPagamentoRepository.Dispose();
            _usuarioRepository.Dispose();
        }
    }
}
