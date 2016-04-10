using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resources.Resources;

namespace ProjetoArtCouro.Model.Models.Estoque
{
    public class EstoqueModel
    {
        [Display(Name = "ProductCode", ResourceType = typeof(Mensagens))]
        public int? CodigoProduto { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Mensagens))]
        public string Descricao { get; set; }

        [Display(Name = "ProviderName", ResourceType = typeof(Mensagens))]
        public string NomeFornecedor { get; set; }

        [Display(Name = "ProviderCode", ResourceType = typeof(Mensagens))]
        public int? CodigoFornecedor { get; set; }

        [Display(Name = "PriceCost", ResourceType = typeof(Mensagens))]
        public string PrecoCusto { get; set; }

        [Display(Name = "PriceSale", ResourceType = typeof(Mensagens))]
        public string PrecoVenda { get; set; }

        [Display(Name = "QuantityInStock", ResourceType = typeof(Mensagens))]
        public int QuantidaEstoque { get; set; }
    }
}
