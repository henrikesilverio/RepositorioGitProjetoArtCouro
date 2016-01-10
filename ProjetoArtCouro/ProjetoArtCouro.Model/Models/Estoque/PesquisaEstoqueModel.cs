using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Estoque
{
    public class PesquisaEstoqueModel
    {
        [Display(Name = "ProductCode", ResourceType = typeof(Mensagens))]
        public int? CodigoProduto { get; set; }

        [Display(Name = "ProductDescription", ResourceType = typeof(Mensagens))]
        public string DescricaoProduto { get; set; }

        [Display(Name = "ProviderName", ResourceType = typeof(Mensagens))]
        public string NomeFornecedor { get; set; }

        [Display(Name = "ProviderCode", ResourceType = typeof(Mensagens))]
        public int? CodigoFornecedor { get; set; }

        [Display(Name = "QuantityInStock", ResourceType = typeof(Mensagens))]
        public int? QuantidaEstoque { get; set; }
    }
}
