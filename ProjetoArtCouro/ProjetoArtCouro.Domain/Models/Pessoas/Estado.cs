using ProjetoArtCouro.Domain.Models.Common;

namespace ProjetoArtCouro.Domain.Models.Pessoas
{
    public class Estado : Lookup
    {
        public virtual Endereco Endereco { get; set; }
    }
}
