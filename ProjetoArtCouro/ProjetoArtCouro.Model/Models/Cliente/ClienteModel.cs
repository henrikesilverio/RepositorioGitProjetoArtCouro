using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Cliente
{
    public class ClienteModel
    {
        [Display(Name = "Code", ResourceType = typeof(Mensagens))]
        public int? Codigo { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Mensagens))]
        public string Nome { get; set; }

        [Display(Name = "CPF", ResourceType = typeof(Mensagens))]
        public string CPF { get; set; }

        [Display(Name = "RG", ResourceType = typeof(Mensagens))]
        public string RG { get; set; }

        [Display(Name = "Sex", ResourceType = typeof(Mensagens))]
        public string Sexo { get; set; }

        [Display(Name = "MaritalStatus", ResourceType = typeof(Mensagens))]
        public int? EstadoCivilId { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Mensagens))]
        public string RazaoSocial { get; set; }

        [Display(Name = "CNPJ", ResourceType = typeof(Mensagens))]
        public string CNPJ { get; set; }

        [Display(Name = "Contact", ResourceType = typeof(Mensagens))]
        public string Contato { get; set; }

        public int PapelPessoa { get; set; }

        public EnderecoModel Endereco { get; set; }

        public MeioComunicacaoModel MeioComunicacao { get; set; }
    }
}
