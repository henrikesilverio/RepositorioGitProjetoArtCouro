using ProjetoArtCouro.Domain.Models.Common;

namespace ProjetoArtCouro.Domain.Models.Pessoas
{
    public class EstadoCivil : Lookup
    {
        public virtual PessoaFisica PessoaFisica { get; set; }
    }
}
