using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using ProjetoArtCouro.Domain.Contracts.IRepository.ICompra;
using ProjetoArtCouro.Domain.Contracts.IRepository.IEstoque;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPagamento;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
using ProjetoArtCouro.Domain.Contracts.IRepository.IProduto;
using ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario;
using ProjetoArtCouro.Domain.Contracts.IService.ICompra;
using ProjetoArtCouro.Domain.Models.Compras;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Estoques;
using ProjetoArtCouro.Resources.Resources;
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
        private readonly IContaPagarRepository _contaPagarRepository;
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IProdutoRepository _produtoRepository;

        public CompraService(ICompraRepository compraRepository, IItemCompraRepository itemCompraRepository,
            IPessoaRepository pessoaRepository, IFormaPagamentoRepository formaPagamentoRepository,
            ICondicaoPagamentoRepository condicaoPagamentoRepository, IUsuarioRepository usuarioRepository,
            IContaPagarRepository contaPagarRepository, IEstoqueRepository estoqueRepository,
            IProdutoRepository produtoRepository)
        {
            _compraRepository = compraRepository;
            _itemCompraRepository = itemCompraRepository;
            _pessoaRepository = pessoaRepository;
            _formaPagamentoRepository = formaPagamentoRepository;
            _condicaoPagamentoRepository = condicaoPagamentoRepository;
            _usuarioRepository = usuarioRepository;
            _contaPagarRepository = contaPagarRepository;
            _estoqueRepository = estoqueRepository;
            _produtoRepository = produtoRepository;
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
                AdicionaContaPagarNaCompra(compraAtual);
                compraAtual.ContasPagar.ForEach(x => x.Validar());
                AdicionarNoEstoque(compra.ItensCompra, compra);
            }
            else
            {
                compraAtual.StatusCompra = StatusCompraEnum.Cancelado;
                RemoveContaPagarDaCompra(compraAtual);
                AtualizarDoEstoque(compraAtual.ItensCompra);
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
            var valorTotalLiquidoSemFrete = compra.ItensCompra.Sum(x => x.ValorLiquido);
            var valorTotalLiquidoComFrete = valorTotalLiquidoSemFrete + compra.ValorTotalFrete;
            AssertionConcern.AssertArgumentEquals(valorTotalLiquidoComFrete, compra.ValorTotalLiquido,
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

        private static void AdicionaContaPagarNaCompra(Compra compra)
        {
            compra.ContasPagar = new List<ContaPagar>();
            for (var i = 0; i < compra.CondicaoPagamento.QuantidadeParcelas; i++)
            {
                compra.ContasPagar.Add(new ContaPagar
                {
                    DataVencimento = DateTime.Now.AddDays(1).AddMonths(i),
                    Pago = false,
                    StatusContaPagar = StatusContaPagarEnum.Aberto,
                    ValorDocumento = compra.ValorTotalLiquido / compra.CondicaoPagamento.QuantidadeParcelas,
                    Compra = compra
                });
            }
        }

        private void RemoveContaPagarDaCompra(Compra compra)
        {
            var contasReceber = _contaPagarRepository.ObterListaPorCodigoCompra(compra.CompraCodigo);
            contasReceber.ForEach(x =>
            {
                _contaPagarRepository.Deletar(x);
            });
        }

        private void AdicionarNoEstoque(IEnumerable<ItemCompra> itensCompra, Compra compra)
        {
            itensCompra.ForEach(x =>
            {
                var estoqueAtual = _estoqueRepository.ObterPorCodigoProduto(x.ProdutoCodigo);
                if (estoqueAtual != null)
                {
                    estoqueAtual.Compra = _compraRepository.ObterPorCodigo(compra.CompraCodigo);
                    estoqueAtual.DataUltimaEntrada = DateTime.Now;
                    estoqueAtual.Quantidade += x.Quantidade;
                    _estoqueRepository.Atualizar(estoqueAtual);
                }
                else
                {
                    _estoqueRepository.Criar(new Estoque
                    {
                        DataUltimaEntrada = DateTime.Now,
                        Compra = _compraRepository.ObterPorCodigo(compra.CompraCodigo),
                        Produto = _produtoRepository.ObterPorCodigo(x.ProdutoCodigo),
                        Quantidade = x.Quantidade
                    });
                }
            });
        }

        private void AtualizarDoEstoque(IEnumerable<ItemCompra> itensCompra)
        {
            itensCompra.ForEach(x =>
            {
                var estoque = _estoqueRepository.ObterPorCodigoProduto(x.ProdutoCodigo);
                estoque.Quantidade -= x.Quantidade;
                _estoqueRepository.Atualizar(estoque);
            });
        }

        public void Dispose()
        {
            _compraRepository.Dispose();
            _itemCompraRepository.Dispose();
            _pessoaRepository.Dispose();
            _formaPagamentoRepository.Dispose();
            _condicaoPagamentoRepository.Dispose();
            _usuarioRepository.Dispose();
            _contaPagarRepository.Dispose();
            _estoqueRepository.Dispose();
            _produtoRepository.Dispose();
        }
    }
}
