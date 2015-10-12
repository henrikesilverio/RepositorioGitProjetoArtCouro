using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa
{
    public interface IPessoaJuridicaRepository : IDisposable
    {
        PessoaJuridica ObterPorId(Guid id);
        PessoaJuridica ObterPorCNPJ(string cnpj);
        List<PessoaJuridica> ObterLista();
        List<PessoaJuridica> ObterLista(int codigo, string nome, string cnpj, string email, TipoPapelPessoaEnum papelCodigo);
        void Criar(PessoaJuridica pessoaJuridica);
        void Atualizar(PessoaJuridica pessoaJuridica);
        void Deletar(PessoaJuridica pessoaJuridica);
    }
}
