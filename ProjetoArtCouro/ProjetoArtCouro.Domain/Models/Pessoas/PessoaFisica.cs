using System;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Domain.Models.Pessoas
{
    public class PessoaFisica
    {
        public Guid PessoaFisicaId { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string Sexo { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual EstadoCivil EstadoCivil { get; set; }

        public void Validar()
        {
            AssertionConcern.AssertArgumentNotEmpty(CPF, Erros.EmptyCPF);
            AssertionConcern.AssertArgumentNotEmpty(RG, Erros.EmptyRG);
            AssertionConcern.AssertArgumentNotEmpty(Sexo, Erros.EmptySex);
            AssertionConcern.AssertArgumentNotNull(Pessoa, Erros.EmptyPerson);
            AssertionConcern.AssertArgumentNotNull(EstadoCivil, Erros.EmptyMaritalStatus);
            Pessoa.Validar();
        }
    }
}
