using System;
using ProjetoArtCouro.Domain.Models.Enums;

namespace ProjetoArtCouro.Domain.Models.Pessoas
{
    public class MeioComunicacao
    {
        public Guid MeioComunicacaoId { get; set; }
        public int MeioComunicacaoCodigo { get; set; }
        public string MeioComunicacaoNome { get; set; }
        public TipoComunicacaoEnum TipoComunicacao { get; set; }
        public bool Principal { get; set; }
        public Guid PessoaId { get; set; }
        public virtual Pessoa Pessoa { get; set; }
    }
}
