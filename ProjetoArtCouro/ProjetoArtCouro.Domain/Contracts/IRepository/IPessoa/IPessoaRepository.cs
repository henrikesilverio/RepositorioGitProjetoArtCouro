using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa
{
    public interface IPessoaRepository : IDisposable 
    {
        Pessoa ObterPorId(Guid id);
        Pessoa ObterPorCodigo(int codigo);
        Pessoa ObterPorCodigoComPessoaCompleta(int codigo);
        List<Pessoa> ObterLista();
        void Criar(Pessoa pessoa);
        void Atualizar(Pessoa pessoa);
        void Deletar(Pessoa pessoa);
    }
}
