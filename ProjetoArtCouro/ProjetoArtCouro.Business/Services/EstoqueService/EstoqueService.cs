using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Contracts.IRepository.IEstoque;
using ProjetoArtCouro.Domain.Contracts.IService.IEstoque;
using ProjetoArtCouro.Domain.Models.Estoques;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Business.Services.EstoqueService
{
    public class EstoqueService : IEstoqueService
    {
        private readonly IEstoqueRepository _estoqueRepository;
        public EstoqueService(IEstoqueRepository estoqueRepository)
        {
            _estoqueRepository = estoqueRepository;
        }

        public List<Estoque> PesquisarEstoque(string descricaoProduto, int codigoProduto, int quantidaEstoque, string nomeFornecedor,
            int codigoFornecedor)
        {
            if (string.IsNullOrEmpty(descricaoProduto) && codigoProduto.Equals(0) && 
                quantidaEstoque.Equals(0) && string.IsNullOrEmpty(nomeFornecedor) &&
                codigoFornecedor.Equals(0))
            {
                throw new Exception(Erros.EmptyParameters);
            }

            return _estoqueRepository.ObterLista(descricaoProduto, codigoProduto, quantidaEstoque, nomeFornecedor,
                codigoFornecedor);
        }

        public void Dispose()
        {
            _estoqueRepository.Dispose();
        }
    }
}
