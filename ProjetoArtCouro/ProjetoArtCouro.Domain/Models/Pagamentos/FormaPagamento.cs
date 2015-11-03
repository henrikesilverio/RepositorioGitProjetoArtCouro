using System;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Domain.Models.Pagamentos
{
    public class FormaPagamento
    {
        public Guid FormaPagamentoId { get; set; }
        public int FormaPagamentoCodigo { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public void Validar()
        {
            AssertionConcern.AssertArgumentNotNull(Descricao, string.Format(Erros.NullParameter, "Descricao"));
            AssertionConcern.AssertArgumentNotEmpty(Descricao, string.Format(Erros.EmptyParameter, "Descricao"));
        }
    }
}
