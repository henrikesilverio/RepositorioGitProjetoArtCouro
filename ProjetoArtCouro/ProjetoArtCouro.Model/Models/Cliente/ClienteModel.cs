using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Model.Infra.DataAnnotation;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Cliente
{
    public class ClienteModel
    {
        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Code", ResourceType = typeof(Mensagens))]
        public int? Codigo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Name", ResourceType = typeof(Mensagens))]
        public string Nome { get; set; }

        [CPFValidation(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "InvalidCPF")]
        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "CPF", ResourceType = typeof(Mensagens))]
        public string CPF { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "RG", ResourceType = typeof(Mensagens))]
        public string RG { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Sex", ResourceType = typeof(Mensagens))]
        public string Sexo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "MaritalStatus", ResourceType = typeof(Mensagens))]
        public int? EstadoCivilId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Name", ResourceType = typeof(Mensagens))]
        public string RazaoSocial { get; set; }

        [CNPJValidation(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "InvalidCNPJ")]
        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "CNPJ", ResourceType = typeof(Mensagens))]
        public string CNPJ { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Contact", ResourceType = typeof(Mensagens))]
        public string Contato { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "TypeOfPerson", ResourceType = typeof(Mensagens))]
        public bool EPessoaFisica { get; set; }

        public int PapelPessoa { get; set; }

        public EnderecoModel Endereco { get; set; }

        public MeioComunicacaoModel MeioComunicacao { get; set; }
    }
}
