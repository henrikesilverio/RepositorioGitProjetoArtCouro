using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Usuario
{
    public class ConfiguracaoUsuarioModel
    {
        [Required(ErrorMessageResourceName = "ERR_Name", ErrorMessageResourceType = typeof(Mensagens))]
        [Display(Name = "Users", ResourceType = typeof(Mensagens))]
        public int? UsuarioId { get; set; }

        [Required(ErrorMessageResourceName = "ERR_Name", ErrorMessageResourceType = typeof(Mensagens))]
        [Display(Name = "Permissions", ResourceType = typeof(Mensagens))]
        public string PermissaoId { get; set; }

        public List<PermissaoModel> Permissoes { get; set; }
    }
}
