using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Produtos;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IProduto
{
    public interface IUnidadeRepository : IDisposable
    {
        Unidade ObterPorId(Guid id);
        Unidade ObterPorCodigo(int codigo);
        List<Unidade> ObterLista();
        void Criar(Unidade unidade);
        void Atualizar(Unidade unidade);
        void Deletar(Unidade unidade);
    }
}
