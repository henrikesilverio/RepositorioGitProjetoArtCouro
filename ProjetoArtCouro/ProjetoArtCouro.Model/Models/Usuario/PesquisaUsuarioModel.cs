using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resources.Resources;

namespace ProjetoArtCouro.Model.Models.Usuario
{
    public class PesquisaUsuarioModel
    {
        [Display(Name = "Name", ResourceType = typeof(Mensagens))]
        public string UsuarioNome { get; set; }

        [Display(Name = "Active", ResourceType = typeof(Mensagens))]
        public bool? Ativo { get; set; }

        [Display(Name = "Groups", ResourceType = typeof(Mensagens))]
        public int? GrupoId { get; set; }
    }
}
