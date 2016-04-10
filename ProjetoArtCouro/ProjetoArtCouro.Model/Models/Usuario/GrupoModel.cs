using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resources.Resources;

namespace ProjetoArtCouro.Model.Models.Usuario
{
    public class GrupoModel
    {
        [Display(Name = "GroupCode", ResourceType = typeof(Mensagens))]
        public int? GrupoCodigo { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "GroupName", ResourceType = typeof(Mensagens))]
        public string GrupoNome { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Permissions", ResourceType = typeof(Mensagens))]
        public string PermissaoId { get; set; }

        public List<PermissaoModel> Permissoes { get; set; }
    }
}
