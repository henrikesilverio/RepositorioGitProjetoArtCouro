using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Produto
{
    public class ProdutoModel
    {
        public int? ProdutoCodigo { get; set; }
        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Description", ResourceType = typeof(Mensagens))]
        public string Descricao { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Unit", ResourceType = typeof(Mensagens))]
        public int UnidadeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Unit", ResourceType = typeof(Mensagens))]
        public string UnidadeNome { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "PriceCost", ResourceType = typeof(Mensagens))]
        public string PrecoCusto { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "PriceSale", ResourceType = typeof(Mensagens))]
        public string PrecoVenda { get; set; }
    }
}
