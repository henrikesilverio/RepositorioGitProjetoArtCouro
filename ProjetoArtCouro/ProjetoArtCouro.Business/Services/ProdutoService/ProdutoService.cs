using System.Collections.Generic;
using ProjetoArtCouro.Domain.Contracts.IRepository.IProduto;
using ProjetoArtCouro.Domain.Contracts.IService.IProduto;
using ProjetoArtCouro.Domain.Models.Produtos;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Business.Services.ProdutoService
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUnidadeRepository _unidadeRepository;
        public ProdutoService(IProdutoRepository produtoRepository, IUnidadeRepository unidadeRepository)
        {
            _produtoRepository = produtoRepository;
            _unidadeRepository = unidadeRepository;
        }

        public List<Produto> ObterListaProduto()
        {
            return _produtoRepository.ObterListaComUnidade();
        }

        public List<Unidade> ObterListaUnidade()
        {
            return _unidadeRepository.ObterLista();
        }

        public Produto ObterProdutoPorCodigo(int codigo)
        {
            return _produtoRepository.ObterPorCodigo(codigo);
        }

        public Produto CriarProduto(Produto produto)
        {
            produto.Validar();
            produto.Unidade.Validar();
            var unidade = _unidadeRepository.ObterPorCodigo(produto.Unidade.UnidadeCodigo);
            AssertionConcern.AssertArgumentNotEquals(unidade, null, Erros.UnitDoesNotExist);
            produto.Unidade = unidade;
            return _produtoRepository.Criar(produto);
        }

        public Produto AtualizarProduto(Produto produto)
        {
            produto.Validar();
            AssertionConcern.AssertArgumentNotEquals(0, produto.ProdutoCodigo, string.Format(Erros.NotZeroParameter, "ProdutoCodigo"));
            produto.Unidade.Validar();
            var unidade = _unidadeRepository.ObterPorCodigo(produto.Unidade.UnidadeCodigo);
            AssertionConcern.AssertArgumentNotEquals(unidade, null, Erros.UnitDoesNotExist);
            var produtoAtual = _produtoRepository.ObterComUnidadePorCodigo(produto.ProdutoCodigo);
            AssertionConcern.AssertArgumentNotEquals(produtoAtual, null, Erros.ProductDoesNotExist);
            produtoAtual.PrecoCusto = produto.PrecoCusto;
            produtoAtual.PrecoVenda = produto.PrecoVenda;
            produtoAtual.ProdutoNome = produto.ProdutoNome;
            produtoAtual.Unidade = unidade;
            return _produtoRepository.Atualizar(produtoAtual);
        }

        public void ExcluirProduto(int produtoCodigo)
        {
            var produtoAtual = _produtoRepository.ObterPorCodigo(produtoCodigo);
            AssertionConcern.AssertArgumentNotEquals(produtoAtual, null, Erros.ProductDoesNotExist);
            _produtoRepository.Deletar(produtoAtual);
        }

        public void Dispose()
        {
            _produtoRepository.Dispose();
            _unidadeRepository.Dispose();
        }
    }
}
