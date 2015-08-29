using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.CondicaoPagamento
{
    public class CondicaoPagamento
    {
        [Display(Name = "Description", ResourceType = typeof(Mensagens))]
        public string Descricao { get; set; }

        [Display(Name = "Active", ResourceType = typeof(Mensagens))]
        public bool Ativo { get; set; }

        [Display(Name = "QuantityOfParcels", ResourceType = typeof(Mensagens))]
        public int QuantidadeParcelas { get; set; }
    }
}
