using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Usuario
{
    public class PesquisaUsuarioModel
    {
        [Display(Name = "Name", ResourceType = typeof(Mensagens))]
        public string UsuarioNome { get; set; }

        [Display(Name = "Active", ResourceType = typeof(Mensagens))]
        public bool Ativo { get; set; }

        [Display(Name = "Permissions", ResourceType = typeof(Mensagens))]
        public int? PermissaoId { get; set; }
    }
}
