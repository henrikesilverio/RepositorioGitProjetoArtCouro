using System;
using System.Collections.Generic;

namespace ProjetoArtCouro.Domain.Models.Pessoas
{
    public class EstadoCivil
    {
        public Guid EstadoCivilId { get; set; }
        public int EstadoCivilCodigo { get; set; }
        public string EstadoCivilNome { get; set; }
        public virtual ICollection<PessoaFisica> PessoaFisica { get; set; }
    }
}
