using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Common;

namespace ProjetoArtCouro.Domain.Models.Pessoas
{
    public class Papel : Lookup
    {
        public virtual ICollection<Pessoa> Pessoas { get; set; }
    }
}
