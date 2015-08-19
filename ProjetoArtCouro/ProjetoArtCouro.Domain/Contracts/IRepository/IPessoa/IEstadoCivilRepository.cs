using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa
{
    public interface IEstadoCivilRepository : IDisposable
    {
        EstadoCivil ObterPorId(Guid id);
        EstadoCivil ObterPorCodigo(int codigo);
        List<EstadoCivil> ObterLista();
        void Criar(EstadoCivil estadoCivil);
        void Atualizar(EstadoCivil estadoCivil);
        void Deletar(EstadoCivil estadoCivil);
    }
}
