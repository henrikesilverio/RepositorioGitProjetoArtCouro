using System;

namespace ProjetoArtCouro.Domain.Models.Pessoas
{
    public class PessoaJuridica
    {
        public Guid PessoaJuridicaId { get; set; }
        public string CNPJ { get; set; }
        public string Contato { get; set; }
        public virtual Pessoa Pessoa { get; set; }
    }
}
