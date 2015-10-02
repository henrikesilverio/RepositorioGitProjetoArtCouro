using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Usuario
{
    public class PesquisaGrupoModel
    {
        [Display(Name = "GroupCode", ResourceType = typeof(Mensagens))]
        public int? GrupoCodigo { get; set; }

        [Display(Name = "GroupName", ResourceType = typeof(Mensagens))]
        public string GrupoNome { get; set; }

        [Display(Name = "EveryoneGroup", ResourceType = typeof(Mensagens))]
        public bool Todos { get; set; }
    }
}
