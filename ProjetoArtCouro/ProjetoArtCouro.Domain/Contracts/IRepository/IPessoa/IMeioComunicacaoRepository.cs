using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa
{
    public interface IMeioComunicacaoRepository : IDisposable
    {
        MeioComunicacao ObterPorId(Guid id);
        MeioComunicacao ObterPorCodigo(int codigo);
        List<MeioComunicacao> ObterLista();
        MeioComunicacao Criar(MeioComunicacao meioComunicacao);
        void Atualizar(MeioComunicacao meioComunicacao);
        void Deletar(MeioComunicacao meioComunicacao);
    }
}
