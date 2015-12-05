using System.Collections.Generic;
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

        public List<LookupModel> Telefones { get; set; }

        [Display(Name = "CellPhones", ResourceType = typeof(Mensagens))]
        public int? CelularId { get; set; }

        [Display(Name = "NewCellPhone", ResourceType = typeof(Mensagens))]
        public string Celular { get; set; }

        public List<LookupModel> Celulares { get; set; }

        [Display(Name = "Emails", ResourceType = typeof(Mensagens))]
        public int? EmailId { get; set; }

        [Display(Name = "NewEmail", ResourceType = typeof(Mensagens))]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", 
            ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }

        public List<LookupModel> Emalis { get; set; }
    }
}
