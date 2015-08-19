using System;
using System.Collections.Generic;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Domain.Models.Pessoas
{
    public class Pessoa
    {
        public Guid PessoaId { get; set; }
        public int PessoaCodigo { get; set; }
        public string Nome { get; set; }
        public Guid MeioComunicacaoId { get; set; }
        public Guid EnderecoId { get; set; }
        public virtual PessoaFisica PessoaFisica { get; set; }
        public virtual PessoaJuridica PessoaJuridica { get; set; }
        public virtual ICollection<Papel> Papeis { get; set; }
        public virtual ICollection<MeioComunicacao> MeiosComunicacao { get; set; }
        public virtual ICollection<Endereco> Enderecos { get; set; }

        public void Validar()
        {
            AssertionConcern.AssertArgumentNotEmpty(Nome, Erros.EmptyCPF);
            AssertionConcern.AssertArgumentNotNull(Papeis, Erros.PaperEmptyPerson);
            AssertionConcern.AssertArgumentNotNull(MeiosComunicacao, Erros.MeansOfCommunicationEmpty);
            AssertionConcern.AssertArgumentNotNull(Enderecos, Erros.EmptyAddress);
        }
    }
}
