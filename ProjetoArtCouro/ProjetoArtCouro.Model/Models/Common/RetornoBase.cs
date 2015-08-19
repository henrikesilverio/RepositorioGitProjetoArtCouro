using System;

namespace ProjetoArtCouro.Model.Models.Common
{
    public class RetornoBase
    {
        public string Mensagem { get; set; }
        public bool TemErros { get; set; }
        public Object ObjetoRetorno { get; set; }
    }
}
