using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.Domain.Contracts.IService.IPessoa
{
    public interface IPessoaService : IDisposable
    {
        void CriarPessoaFisica(Pessoa pessoa);
        List<PessoaFisica> PesquisarPessoaFisica(int codigo, string nome, string cpf, string email);
        List<Estado> ObterEstados();
        List<EstadoCivil> ObterEstadosCivis();
    }
}
