using System;
using System.Collections.Generic;

namespace ProjetoArtCouro.Domain.Models.Pessoas
{
    public class Estado
    {
        public Guid EstadoId { get; set; }
        public int EstadoCodigo { get; set; }
        public string EstadoNome { get; set; }
        public virtual ICollection<Endereco> Endereco { get; set; }
    }
}
