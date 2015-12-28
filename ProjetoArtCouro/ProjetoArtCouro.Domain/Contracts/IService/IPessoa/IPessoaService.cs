using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.Domain.Contracts.IService.IPessoa
{
    public interface IPessoaService : IDisposable
    {
        List<Estado> ObterEstados();
        List<EstadoCivil> ObterEstadosCivis();
        List<Pessoa> ObterListaPessoa();
        List<Pessoa> ObterListaPessoaFisicaEJuridicaPorPapel(TipoPapelPessoaEnum papelCodigo);
        List<PessoaFisica> ObterListaPessoaFisicaPorPapel(TipoPapelPessoaEnum papelCodigo);
        List<PessoaJuridica> ObterListaPessoaJuridicaPorPapel(TipoPapelPessoaEnum papelCodigo);
        List<PessoaFisica> PesquisarPessoaFisica(int codigo, string nome, string cpf, string email, TipoPapelPessoaEnum papelCodigo);
        List<PessoaJuridica> PesquisarPessoaJuridica(int codigo, string nome, string cnpj, string email, TipoPapelPessoaEnum papelCodigo);
        Pessoa ObterPessoaPorCodigo(int codigo);
        void CriarPessoaFisica(Pessoa pessoa);
        void CriarPessoaJuridica(Pessoa pessoa);
        void AtualizarPessoa(Pessoa pessoa);
        void ExcluirPessoa(int pessoaCodigo);
    }
}
