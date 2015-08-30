using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Produto
{
    public class ProdutoModel
    {
        [Display(Name = "Description", ResourceType = typeof(Mensagens))]
        public string Descricao { get; set; }

        [Display(Name = "Unit", ResourceType = typeof(Mensagens))]
        public int UnidadeId { get; set; }

        [Display(Name = "Unit", ResourceType = typeof(Mensagens))]
        public int UnidadeNome { get; set; }

        [Display(Name = "PriceCost", ResourceType = typeof(Mensagens))]
        public double PrecoCusto { get; set; }

        [Display(Name = "PriceSale", ResourceType = typeof(Mensagens))]
        public double PrecoVenda { get; set; }
    }
}
