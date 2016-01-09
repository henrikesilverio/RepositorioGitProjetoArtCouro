using System;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Domain.Models.Compras
{
    public class ContaPagar
    {
        public Guid ContaPagarId { get; set; }
        public int ContaPagarCodigo { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorDocumento { get; set; }
        public StatusContaPagarEnum StatusContaPagar { get; set; }
        public bool Recebido { get; set; }
        public virtual Compra Compra { get; set; }

        public void Validar()
        {
            AssertionConcern.AssertArgumentNotEquals(new DateTime(), DataVencimento,
                string.Format(Erros.InvalidParameter, "DataVencimento"));
            AssertionConcern.AssertArgumentNotEquals(0.0M, ValorDocumento,
                string.Format(Erros.NotZeroParameter, "ValorDocumento"));
            AssertionConcern.AssertArgumentNotEquals(StatusContaPagar, StatusContaPagarEnum.None,
                string.Format(Erros.InvalidParameter, "StatusContaPagar"));
            AssertionConcern.AssertArgumentNotNull(Compra, Erros.PurchaseNotSet);
        }
    }
}
