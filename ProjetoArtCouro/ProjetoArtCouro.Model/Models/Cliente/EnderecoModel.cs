using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Cliente
{
    public class EnderecoModel
    {
        [Display(Name = "Code", ResourceType = typeof(Mensagens))]
        public int? Codigo { get; set; }

        [Display(Name = "Patio", ResourceType = typeof(Mensagens))]
        public string Logradouro { get; set; }

        [Display(Name = "Neighborhood", ResourceType = typeof(Mensagens))]
        public string Bairro { get; set; }

        [Display(Name = "Number", ResourceType = typeof(Mensagens))]
        public string Numero { get; set; }

        [Display(Name = "Complement", ResourceType = typeof(Mensagens))]
        public string Complemento { get; set; }

        [Display(Name = "City", ResourceType = typeof(Mensagens))]
        public string Cidade { get; set; }

        [Display(Name = "Cep", ResourceType = typeof(Mensagens))]
        public string Cep { get; set; }

        [Display(Name = "State", ResourceType = typeof(Mensagens))]
        public int? UFId { get; set; }
    }
}
