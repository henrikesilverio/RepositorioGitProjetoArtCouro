using System;

namespace ProjetoArtCouro.Domain.Models.Pessoas
{
    public class Endereco
    {
        public Guid EnderecoId { get; set; }
        public int EnderecoCodigo { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public bool Principal { get; set; }
        public Guid PessoaId { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual Pessoa Pessoa { get; set; }
    }
}
