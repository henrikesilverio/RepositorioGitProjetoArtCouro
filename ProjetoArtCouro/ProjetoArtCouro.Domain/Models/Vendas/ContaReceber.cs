using System;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Domain.Models.Vendas
{
    public class ContaReceber
    {
        public Guid ContaReceberId { get; set; }
        public int ContaReceberCodigo { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorDocumento { get; set; }
        public StatusContaReceberEnum StatusContaReceber { get; set; }
        public bool Recebido { get; set; }
        public virtual Venda Venda { get; set; }

        public void Validar()
        {
            AssertionConcern.AssertArgumentNotEquals(new DateTime(), DataVencimento,
                string.Format(Erros.InvalidParameter, "DataVencimento"));
            AssertionConcern.AssertArgumentNotEquals(0.0M, ValorDocumento,
                string.Format(Erros.NotZeroParameter, "ValorDocumento"));
            AssertionConcern.AssertArgumentNotEquals(StatusContaReceber, StatusContaReceberEnum.None,
                string.Format(Erros.InvalidParameter, "StatusContaReceber"));
            AssertionConcern.AssertArgumentNotNull(Venda, Erros.SaleNotSet);
        }
    }
}
