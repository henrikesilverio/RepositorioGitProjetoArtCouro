using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa
{
    public interface IEstadoRepository : IDisposable
    {
        Estado ObterPorId(Guid id);
        Estado ObterPorCodigo(int codigo);
        List<Estado> ObterLista();
        void Criar(Estado estado);
        void Atualizar(Estado estado);
        void Deletar(Estado estado);
    }
}
