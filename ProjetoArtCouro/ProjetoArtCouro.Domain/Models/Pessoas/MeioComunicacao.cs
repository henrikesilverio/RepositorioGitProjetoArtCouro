using System;
using ProjetoArtCouro.Domain.Models.Common;
using ProjetoArtCouro.Domain.Models.Enums;

namespace ProjetoArtCouro.Domain.Models.Pessoas
{
    public class MeioComunicacao : Lookup
    {
        public TipoComunicacaoEnum TipoComunicacao { get; set; }
        public bool Principal { get; set; }
        public Guid PessoaId { get; set; }
        public virtual Pessoa Pessoa { get; set; }
    }
}
