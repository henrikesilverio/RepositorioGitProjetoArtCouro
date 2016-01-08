using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pagamentos;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Domain.Models.Vendas
{
    public class Venda
    {
        public Guid VendaId { get; set; }
        public int VendaCodigo { get; set; }
        public DateTime DataCadastro { get; set; }
        public StatusVendaEnum StatusVenda { get; set; }
        public decimal ValorTotalBruto { get; set; }
        public decimal ValorTotalDesconto { get; set; }
        public decimal ValorTotalLiquido { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Pessoa Cliente { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual CondicaoPagamento CondicaoPagamento { get; set; }
        public virtual ICollection<ItemVenda> ItensVenda { get; set; }
        public virtual ICollection<ContaReceber> ContasReceber { get; set; }

        public void Validar()
        {
            AssertionConcern.AssertArgumentNotEquals(new DateTime(), DataCadastro,
                string.Format(Erros.InvalidParameter, "DataCadastro"));
            AssertionConcern.AssertArgumentNotEquals(StatusVenda, StatusVendaEnum.None,
                string.Format(Erros.InvalidParameter, "StatusVenda"));
            AssertionConcern.AssertArgumentNotEquals(0.0M, ValorTotalBruto,
                string.Format(Erros.NotZeroParameter, "ValorTotalBruto"));
            AssertionConcern.AssertArgumentNotEquals(0.0M, ValorTotalLiquido,
                string.Format(Erros.NotZeroParameter, "ValorTotalLiquido"));
            AssertionConcern.AssertArgumentNotEquals(0, Usuario.UsuarioCodigo, Erros.UserNotSet);
            AssertionConcern.AssertArgumentTrue(ItensVenda.Any(), Erros.SaleItemsNotSet);
        }
    }
}
