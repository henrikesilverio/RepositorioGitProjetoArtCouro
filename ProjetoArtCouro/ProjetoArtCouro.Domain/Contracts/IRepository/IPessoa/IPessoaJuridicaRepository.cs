using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa
{
    public interface IPessoaJuridicaRepository : IDisposable
    {
        PessoaJuridica ObterPorId(Guid id);
        List<PessoaJuridica> ObterLista();
        void Criar(PessoaJuridica pessoaJuridica);
        void Atualizar(PessoaJuridica pessoaJuridica);
        void Deletar(PessoaJuridica pessoaJuridica);
    }
}
