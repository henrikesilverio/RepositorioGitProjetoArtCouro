using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Compra
{
    public class ItemCompraModel
    {
        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Code", ResourceType = typeof(Mensagens))]
        public int? Codigo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Description", ResourceType = typeof(Mensagens))]
        public string Descricao { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Quantity", ResourceType = typeof(Mensagens))]
        public int? Quantidade { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "PriceSale", ResourceType = typeof(Mensagens))]
        public string PrecoVenda { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "ValueCrude", ResourceType = typeof(Mensagens))]
        public string ValorBruto { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "ValueFreight", ResourceType = typeof(Mensagens))]
        public string ValorFrete { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "ValueLiquid", ResourceType = typeof(Mensagens))]
        public string ValorLiquido { get; set; }
    }
}
