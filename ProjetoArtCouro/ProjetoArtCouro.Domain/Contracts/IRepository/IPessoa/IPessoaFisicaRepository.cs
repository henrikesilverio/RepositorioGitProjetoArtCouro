using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa
{
    public interface IPessoaFisicaRepository: IDisposable
    {
        PessoaFisica ObterPorId(Guid id);
        PessoaFisica ObterPorCPF(string cpf);
        List<PessoaFisica> ObterLista();
        List<PessoaFisica> ObterLista(int codigo, string nome, string cpf, string email, TipoPapelPessoaEnum papelCodigo); 
        void Criar(PessoaFisica pessoaFisica);
        void Atualizar(PessoaFisica pessoaFisica);
        void Deletar(PessoaFisica pessoaFisica);
    }
}
