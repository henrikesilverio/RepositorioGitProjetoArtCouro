using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using ProjetoArtCouro.Domain.Contracts.IRepository.IEstoque;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPagamento;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
using ProjetoArtCouro.Domain.Contracts.IRepository.IProduto;
using ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario;
using ProjetoArtCouro.Domain.Contracts.IRepository.IVenda;
using ProjetoArtCouro.Domain.Contracts.IService.IVenda;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Vendas;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Business.Services.VendaService
{
    public class VendaService : IVendaService
    {
        private readonly IVendaRepository _vendaRepository;
        private readonly IItemVendaRepository _itemVendaRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IFormaPagamentoRepository _formaPagamentoRepository;
        private readonly ICondicaoPagamentoRepository _condicaoPagamentoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IContaReceberRepository _contaReceberRepository;
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IProdutoRepository _produtoRepository;

        public VendaService(IVendaRepository vendaRepository, IItemVendaRepository itemVendaRepository,
            IPessoaRepository pessoaRepository, IFormaPagamentoRepository formaPagamentoRepository,
            ICondicaoPagamentoRepository condicaoPagamentoRepository, IUsuarioRepository usuarioRepository,
            IContaReceberRepository contaReceberRepository, IEstoqueRepository estoqueRepository,
            IProdutoRepository produtoRepository)
        {
            _vendaRepository = vendaRepository;
            _itemVendaRepository = itemVendaRepository;
            _pessoaRepository = pessoaRepository;
            _formaPagamentoRepository = formaPagamentoRepository;
            _condicaoPagamentoRepository = condicaoPagamentoRepository;
            _usuarioRepository = usuarioRepository;
            _contaReceberRepository = contaReceberRepository;
            _estoqueRepository = estoqueRepository;
            _produtoRepository = produtoRepository;
        }

        public Venda ObterVendaPorCodigo(int codigoVenda)
        {
            return _vendaRepository.ObterPorCodigoComItensVenda(codigoVenda);
        }

        public List<Venda> PesquisarVenda(int codigoVenda, int codigoCliente, DateTime dataCadastro, int statusVenda,
            string nomeCliente, string documento, int codigoUsuario)
        {
            if (codigoVenda.Equals(0) && codigoCliente.Equals(0) &&
                dataCadastro.Equals(new DateTime()) && statusVenda.Equals(0) &&
                string.IsNullOrEmpty(nomeCliente) && string.IsNullOrEmpty(documento) &&
                codigoUsuario.Equals(0))
            {
                throw new Exception(Erros.EmptyParameters);
            }

            return _vendaRepository.ObterLista(codigoVenda, codigoCliente, dataCadastro, statusVenda, nomeCliente,
                documento, codigoUsuario);
        }

        public void CriarVenda(Venda venda)
        {
            venda.Validar();
            venda.ItensVenda.ForEach(x => x.Validar());
            AssertionConcern.AssertArgumentEquals(venda.StatusVenda, StatusVendaEnum.Aberto, Erros.StatusOfDifferentSalesOpen);
            AplicaValidacoesPadrao(venda);
            var usuario = _usuarioRepository.ObterPorCodigo(venda.Usuario.UsuarioCodigo);
            venda.Usuario = usuario;
            venda.Cliente = null;
            venda.FormaPagamento = null;
            venda.CondicaoPagamento = null;
            _vendaRepository.Criar(venda);
        }

        public void AtualizarVenda(Venda venda)
        {
            venda.Validar();
            venda.ItensVenda.ForEach(x => x.Validar());
            AssertionConcern.AssertArgumentNotEquals(0, venda.VendaCodigo,
                string.Format(Erros.NotZeroParameter, "VendaCodigo"));
            VerificarEstoque(venda.ItensVenda);
            var vendaAtual = _vendaRepository.ObterPorCodigoComItensVenda(venda.VendaCodigo);
            if (venda.StatusVenda == StatusVendaEnum.Aberto)
            {
                AplicaValidacoesPadrao(venda);
                AdicionaClienteFormaECondicaoDePagamento(venda, vendaAtual);
                AtualizaItensVenda(vendaAtual, venda.ItensVenda);
                vendaAtual.StatusVenda = StatusVendaEnum.Confirmado;
                AdicionaContaReceberNaVenda(vendaAtual);
                vendaAtual.ContasReceber.ForEach(x => x.Validar());
                BaixarDoEstoque(vendaAtual.ItensVenda);
            }
            else
            {
                vendaAtual.StatusVenda = StatusVendaEnum.Cancelado;
                RemoveContaReceberDaVenda(vendaAtual);
                DevolverNoEstoque(vendaAtual.ItensVenda);
            }
            _vendaRepository.Atualizar(vendaAtual);
        }

        public void ExcluirVenda(int codigoVenda)
        {
            var venda = _vendaRepository.ObterPorCodigo(codigoVenda);
            AssertionConcern.AssertArgumentNotEquals(venda, null, Erros.SaleDoesNotExist);
            AssertionConcern.AssertArgumentNotEquals(venda.StatusVenda, StatusVendaEnum.Confirmado, Erros.SaleConfirmedCanNotBeExcluded);
            _vendaRepository.Deletar(venda);
        }

        private void AtualizaItensVenda(Venda vendaAtual, IEnumerable<ItemVenda> itensVenda)
        {
            var itensVendaAtual = vendaAtual.ItensVenda.ToList();
            itensVendaAtual.ForEach(x =>
            {
                _itemVendaRepository.Deletar(x);
            });
            vendaAtual.ItensVenda.Clear();
            itensVenda.ForEach(x =>
            {
                x.Venda = vendaAtual;
                var novoItem = _itemVendaRepository.Criar(x);
                vendaAtual.ItensVenda.Add(novoItem);
            });
        }

        private static void AplicaValidacoesPadrao(Venda venda)
        {
            AssertionConcern.AssertArgumentEquals(venda.ItensVenda.Sum(x => x.ValorBruto), venda.ValorTotalBruto,
                    Erros.SumDoNotMatchTotalCrudeValue);
            AssertionConcern.AssertArgumentEquals(venda.ItensVenda.Sum(x => x.ValorDesconto),
                venda.ValorTotalDesconto,
                Erros.SumDoNotMatchTotalValueDiscount);
            AssertionConcern.AssertArgumentEquals(venda.ItensVenda.Sum(x => x.ValorLiquido), venda.ValorTotalLiquido,
                Erros.SumDoNotMatchTotalValueLiquid);
        }

        private void AdicionaClienteFormaECondicaoDePagamento(Venda venda, Venda vendaAtual)
        {
            AssertionConcern.AssertArgumentNotEquals(0, venda.Cliente.PessoaCodigo, Erros.ClientNotSet);
            AssertionConcern.AssertArgumentNotEquals(0, venda.FormaPagamento.FormaPagamentoCodigo,
                Erros.NotSetPayment);
            AssertionConcern.AssertArgumentNotEquals(0, venda.CondicaoPagamento.CondicaoPagamentoCodigo,
                Erros.PaymentConditionNotSet);
            var cliente = _pessoaRepository.ObterPorCodigo(venda.Cliente.PessoaCodigo);
            var formaPagamento = _formaPagamentoRepository.ObterPorCodigo(venda.FormaPagamento.FormaPagamentoCodigo);
            var condicaoPagamento =
                _condicaoPagamentoRepository.ObterPorCodigo(venda.CondicaoPagamento.CondicaoPagamentoCodigo);
            vendaAtual.Cliente = cliente;
            vendaAtual.FormaPagamento = formaPagamento;
            vendaAtual.CondicaoPagamento = condicaoPagamento;
        }

        private static void AdicionaContaReceberNaVenda(Venda venda)
        {
            venda.ContasReceber = new List<ContaReceber>();
            for (var i = 0; i < venda.CondicaoPagamento.QuantidadeParcelas; i++)
            {
                venda.ContasReceber.Add(new ContaReceber
                {
                    DataVencimento = DateTime.Now.AddDays(1).AddMonths(i),
                    Recebido = false,
                    StatusContaReceber = StatusContaReceberEnum.Aberto,
                    ValorDocumento = venda.ValorTotalLiquido / venda.CondicaoPagamento.QuantidadeParcelas,
                    Venda = venda
                });
            }
        }

        private void RemoveContaReceberDaVenda(Venda venda)
        {
            var contasReceber = _contaReceberRepository.ObterListaPorCodigoVenda(venda.VendaCodigo);
            contasReceber.ForEach(x =>
            {
                _contaReceberRepository.Deletar(x);
            });
        }

        private void BaixarDoEstoque(IEnumerable<ItemVenda> itensVenda)
        {
            itensVenda.ForEach(x =>
            {
                var estoque = _estoqueRepository.ObterPorCodigoProduto(x.ProdutoCodigo);
                estoque.Quantidade = estoque.Quantidade - x.Quantidade;
                _estoqueRepository.Atualizar(estoque);
            });
        }

        private void DevolverNoEstoque(IEnumerable<ItemVenda> itensVenda)
        {
            itensVenda.ForEach(x =>
            {
                var estoque = _estoqueRepository.ObterPorCodigoProduto(x.ProdutoCodigo);
                estoque.Quantidade = estoque.Quantidade + x.Quantidade;
                _estoqueRepository.Atualizar(estoque);
            });
        }

        private void VerificarEstoque(IEnumerable<ItemVenda> itensVenda)
        {
            var produtosQueFaltamNoEstoque = new List<Tuple<string, int, int>>();
            itensVenda.ForEach(x =>
            {
                var estoque = _estoqueRepository.ObterPorCodigoProduto(x.ProdutoCodigo);
                if (estoque.Quantidade - x.Quantidade < 0)
                {
                    produtosQueFaltamNoEstoque.Add(new Tuple<string, int, int>(x.ProdutoNome, x.Quantidade,
                        estoque.Quantidade));
                }
            });
            AssertionConcern.AssertArgumentFalse(produtosQueFaltamNoEstoque.Any(),
                MensagemDeEstoqueInsuficiente(produtosQueFaltamNoEstoque));
        }

        private static string MensagemDeEstoqueInsuficiente(List<Tuple<string, int, int>> produtosQueFaltamNoEstoque)
        {
            var mensagen = Erros.ProductsThatAreMissingInStock + "</br>";
            produtosQueFaltamNoEstoque.ForEach(x =>
            {
                mensagen += string.Format(Erros.ProductInsufficient, x.Item1, x.Item2, x.Item1) + "</br>";
            });
            return mensagen;
        }

        public void Dispose()
        {
            _vendaRepository.Dispose();
            _itemVendaRepository.Dispose();
            _pessoaRepository.Dispose();
            _formaPagamentoRepository.Dispose();
            _condicaoPagamentoRepository.Dispose();
            _usuarioRepository.Dispose();
            _contaReceberRepository.Dispose();
            _estoqueRepository.Dispose();
            _produtoRepository.Dispose();
        }
    }
}
