using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa
{
    public interface IPessoaFisicaRepository: IDisposable
    {
        PessoaFisica ObterPorId(Guid id);
        List<PessoaFisica> ObterLista();
        List<PessoaFisica> ObterLista(int codigo, string nome, string cpf, string email); 
        void Criar(PessoaFisica pessoaFisica);
        void Atualizar(PessoaFisica pessoaFisica);
        void Deletar(PessoaFisica pessoaFisica);
    }
}
