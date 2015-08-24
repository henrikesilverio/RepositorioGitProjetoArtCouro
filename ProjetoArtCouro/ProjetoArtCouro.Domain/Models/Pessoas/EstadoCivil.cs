using System;

namespace ProjetoArtCouro.Domain.Models.Pessoas
{
    public class EstadoCivil
    {
        public Guid EstadoCivilId { get; set; }
        public int EstadoCivilCodigo { get; set; }
        public string EstadoCivilNome { get; set; }
        public virtual PessoaFisica PessoaFisica { get; set; }
    }
}
