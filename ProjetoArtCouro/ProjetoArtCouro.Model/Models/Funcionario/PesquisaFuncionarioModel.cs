using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Funcionario
{
    public class PesquisaFuncionarioModel
    {
        [Display(Name = "Code", ResourceType = typeof(Mensagens))]
        public int? CodigoFuncionario { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Mensagens))]
        public string Nome { get; set; }

        [Display(Name = "CPFCNPJ", ResourceType = typeof(Mensagens))]
        public string CPFCNPJ { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Mensagens))]
        public string Email { get; set; }

        [Display(Name = "TypeOfPerson", ResourceType = typeof(Mensagens))]
        public bool EPessoaFisica { get; set; }
    }
}
