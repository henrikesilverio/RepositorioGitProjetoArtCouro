using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Produtos;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IProduto
{
    public interface IProdutoRepository : IDisposable
    {
        Produto ObterPorId(Guid id);
        Produto ObterPorCodigo(int codigo);
        Produto ObterComUnidadePorCodigo(int codigo);
        List<Produto> ObterLista();
        List<Produto> ObterListaComUnidade();
        Produto Criar(Produto produto);
        Produto Atualizar(Produto produto);
        void Deletar(Produto produto);
    }
}
