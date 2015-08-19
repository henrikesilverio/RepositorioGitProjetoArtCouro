using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Common;

namespace ProjetoArtCouro.Domain.Models.Usuarios
{
    public class Permissao: Lookup
    {
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
