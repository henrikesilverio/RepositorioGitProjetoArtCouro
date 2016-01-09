using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pagamentos;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Domain.Models.Compras
{
    public class Compra
    {
        public Guid CompraId { get; set; }
        public int CompraCodigo { get; set; }
        public DateTime DataCadastro { get; set; }
        public StatusCompraEnum StatusCompra { get; set; }
        public decimal ValorTotalBruto { get; set; }
        public decimal ValorTotalFrete { get; set; }
        public decimal ValorTotalLiquido { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Pessoa Fornecedor { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual CondicaoPagamento CondicaoPagamento { get; set; }
        public virtual ICollection<ItemCompra> ItensCompra { get; set; }
        public virtual ICollection<ContaPagar> ContasPagar { get; set; }

        public void Validar()
        {
            AssertionConcern.AssertArgumentNotEquals(new DateTime(), DataCadastro,
                string.Format(Erros.InvalidParameter, "DataCadastro"));
            AssertionConcern.AssertArgumentNotEquals(StatusCompra, StatusCompraEnum.None,
                string.Format(Erros.InvalidParameter, "StatusCompra"));
            AssertionConcern.AssertArgumentNotEquals(0.0M, ValorTotalBruto,
                string.Format(Erros.NotZeroParameter, "ValorTotalBruto"));
            AssertionConcern.AssertArgumentNotEquals(0.0M, ValorTotalLiquido,
                string.Format(Erros.NotZeroParameter, "ValorTotalLiquido"));
            AssertionConcern.AssertArgumentNotEquals(0, Usuario.UsuarioCodigo, Erros.UserNotSet);
            AssertionConcern.AssertArgumentTrue(ItensCompra.Any(), Erros.SaleItemsNotSet);
        }
    }
}
