using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Estoques;

namespace ProjetoArtCouro.Domain.Contracts.IService.IEstoque
{
    public interface IEstoqueService : IDisposable
    {
        List<Estoque> PesquisarEstoque(string descricaoProduto, int codigoProduto, int quantidaEstoque,
            string nomeFornecedor, int codigoFornecedor);
    }
}
