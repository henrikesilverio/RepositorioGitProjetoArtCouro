using System.CodeDom;
using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.CondicaoPagamento
{
    public class CondicaoPagamentoModel
    {
        public int? CondicaoPagamentoCodigo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Description", ResourceType = typeof(Mensagens))]
        public string Descricao { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Active", ResourceType = typeof(Mensagens))]
        public bool Ativo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Range(typeof(int), "0", "99", ErrorMessageResourceName = "InvalidQuantityOfParcels", ErrorMessageResourceType = typeof(Erros))]
        [Display(Name = "QuantityOfParcels", ResourceType = typeof(Mensagens))]
        public int QuantidadeParcelas { get; set; }
    }
}
