using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Cliente
{
    public class MeioComunicacaoModel
    {
        [Display(Name = "Phone", ResourceType = typeof(Mensagens))]
        public int? CodigoTelefone { get; set; }

        [Display(Name = "NewPhone", ResourceType = typeof(Mensagens))]
        public string Telefone { get; set; }

        [Display(Name = "CellPhone", ResourceType = typeof(Mensagens))]
        public int? CodigoCelular { get; set; }

        [Display(Name = "NewCellPhone", ResourceType = typeof(Mensagens))]
        public string Celular { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Mensagens))]
        public int? CodigoEmail { get; set; }

        [Display(Name = "NewEmail", ResourceType = typeof(Mensagens))]
        public string Email { get; set; }
    }
}
