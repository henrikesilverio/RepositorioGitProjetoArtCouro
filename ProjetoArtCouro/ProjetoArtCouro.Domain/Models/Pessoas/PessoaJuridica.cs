using System;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Domain.Models.Pessoas
{
    public class PessoaJuridica
    {
        public Guid PessoaId { get; set; }
        public int PessoaJuridicaCodigo { get; set; }
        public string CNPJ { get; set; }
        public string Contato { get; set; }
        public virtual Pessoa Pessoa { get; set; }

        public void Validar()
        {
            AssertionConcern.AssertArgumentNotEmpty(CNPJ, Erros.EmptyCNPJ);
            AssertionConcern.AssertArgumentNotEmpty(Contato, Erros.EmptyContact);
        }
    }
}
