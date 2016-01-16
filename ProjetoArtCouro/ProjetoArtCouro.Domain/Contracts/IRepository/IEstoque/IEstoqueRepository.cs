using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Estoques;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IEstoque
{
    public interface IEstoqueRepository : IDisposable
    {
        Estoque ObterPorId(Guid id);
        Estoque ObterPorCodigo(int codigo);
        Estoque ObterPorCodigoProduto(int codigoProduto);
        List<Estoque> ObterLista();
        List<Estoque> ObterLista(string descricaoProduto, int codigoProduto, int quantidaEstoque, string nomeFornecedor,
            int codigoFornecedor);
        void Criar(Estoque estoque);
        void Atualizar(Estoque estoque);
        void Deletar(Estoque estoque);
    }
}
