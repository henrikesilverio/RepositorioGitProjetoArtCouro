using System;
using ProjetoArtCouro.Domain.Models.Estoques;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Domain.Models.Produtos
{
    public class Produto
    {
        public Guid ProdutoId { get; set; }
        public int ProdutoCodigo { get; set; }
        public string ProdutoNome { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual Estoque Estoque { get; set; }

        public void Validar()
        {
            AssertionConcern.AssertArgumentNotNull(ProdutoNome, string.Format(Erros.NullParameter, "ProdutoNome"));
            AssertionConcern.AssertArgumentNotEmpty(ProdutoNome, string.Format(Erros.EmptyParameter, "ProdutoNome"));
            AssertionConcern.AssertArgumentNotEquals(0.0M, PrecoCusto, string.Format(Erros.NotZeroParameter, "PrecoCusto"));
            AssertionConcern.AssertArgumentNotEquals(0.0M, PrecoVenda, string.Format(Erros.NotZeroParameter, "PrecoVenda"));
            AssertionConcern.AssertArgumentNotNull(Unidade, string.Format(Erros.NullParameter, "Unidade"));
        }
    }
}
