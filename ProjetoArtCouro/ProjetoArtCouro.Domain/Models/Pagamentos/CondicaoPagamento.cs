using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Compras;
using ProjetoArtCouro.Domain.Models.Vendas;
using ProjetoArtCouro.Resources.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Domain.Models.Pagamentos
{
    public class CondicaoPagamento
    {
        public Guid CondicaoPagamentoId { get; set; }
        public int CondicaoPagamentoCodigo { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public int QuantidadeParcelas { get; set; }
        public virtual ICollection<Venda> Vendas { get; set; }
        public virtual ICollection<Compra> Compras { get; set; }

        public void Validar()
        {
            AssertionConcern.AssertArgumentNotNull(Descricao, string.Format(Erros.NullParameter, "Descricao"));
            AssertionConcern.AssertArgumentNotEmpty(Descricao, string.Format(Erros.EmptyParameter, "Descricao"));
            AssertionConcern.AssertArgumentNotEquals(0, QuantidadeParcelas, string.Format(Erros.NotZeroParameter, "QuantidadeParcelas"));
        }
    }
}
