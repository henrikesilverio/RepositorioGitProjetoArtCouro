using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Common
{
    public class MeioComunicacaoModel
    {
        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Phones", ResourceType = typeof(Mensagens))]
        public int? TelefoneId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "NewPhone", ResourceType = typeof(Mensagens))]
        public string Telefone { get; set; }

        [Display(Name = "CellPhones", ResourceType = typeof(Mensagens))]
        public int? CelularId { get; set; }

        [Display(Name = "NewCellPhone", ResourceType = typeof(Mensagens))]
        public string Celular { get; set; }

        [Display(Name = "Emails", ResourceType = typeof(Mensagens))]
        public int? EmailId { get; set; }

        [Display(Name = "NewEmail", ResourceType = typeof(Mensagens))]
        public string Email { get; set; }
    }
}
