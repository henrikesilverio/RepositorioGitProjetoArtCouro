using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa
{
    public interface IEnderecoRepository : IDisposable
    {
        Endereco ObterPorId(Guid id);
        List<Endereco> ObterLista();
        Endereco Criar(Endereco endereco);
        void Atualizar(Endereco endereco);
        void Deletar(Endereco endereco);
    }
}
