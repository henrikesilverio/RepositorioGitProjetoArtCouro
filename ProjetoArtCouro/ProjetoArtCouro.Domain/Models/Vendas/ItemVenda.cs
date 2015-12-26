using System;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Domain.Models.Vendas
{
    public class ItemVenda
    {
        public Guid ItemVendaId { get; set; }
        public int ItemVendaCodigo { get; set; }
        public int ProdutoCodigo { get; set; }
        public string ProdutoNome { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal ValorBruto { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorLiquido { get; set; }
        public virtual Venda Venda { get; set; }

        public void Validar()
        {
            AssertionConcern.AssertArgumentNotEquals(0, ProdutoCodigo, string.Format(Erros.NotZeroParameter, "ProdutoCodigo"));
            AssertionConcern.AssertArgumentNotEmpty(ProdutoNome, string.Format(Erros.EmptyParameter, "ProdutoNome"));
            AssertionConcern.AssertArgumentNotEquals(0, Quantidade, string.Format(Erros.NotZeroParameter, "Quantidade"));
            AssertionConcern.AssertArgumentNotEquals(0.0M, PrecoVenda, string.Format(Erros.NotZeroParameter, "PrecoVenda"));
            AssertionConcern.AssertArgumentNotEquals(0.0M, ValorBruto, string.Format(Erros.NotZeroParameter, "ValorBruto"));
            AssertionConcern.AssertArgumentNotEquals(0.0M, ValorLiquido, string.Format(Erros.NotZeroParameter, "ValorLiquido"));
        }
    }
}
