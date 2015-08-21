using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Cliente
{
    public class EnderecoModel
    {
        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Addresses", ResourceType = typeof(Mensagens))]
        public int? EnderecoId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Patio", ResourceType = typeof(Mensagens))]
        public string Logradouro { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Neighborhood", ResourceType = typeof(Mensagens))]
        public string Bairro { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Number", ResourceType = typeof(Mensagens))]
        public string Numero { get; set; }

        [Display(Name = "Complement", ResourceType = typeof(Mensagens))]
        public string Complemento { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "City", ResourceType = typeof(Mensagens))]
        public string Cidade { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "CEP", ResourceType = typeof(Mensagens))]
        public string Cep { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "State", ResourceType = typeof(Mensagens))]
        public int? UFId { get; set; }
    }
}
