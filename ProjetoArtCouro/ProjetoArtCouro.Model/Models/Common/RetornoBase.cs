using System;

namespace ProjetoArtCouro.Model.Models.Common
{
    public class RetornoBase<T>
    {
        public string Mensagem { get; set; }
        public bool TemErros { get; set; }
        public T ObjetoRetorno { get; set; }
    }
}
