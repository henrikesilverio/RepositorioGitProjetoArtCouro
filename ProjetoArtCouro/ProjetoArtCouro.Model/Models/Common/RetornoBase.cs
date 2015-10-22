using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Common
{
    public class RetornoBase<T>
    {
        public RetornoBase()
        {
            Mensagem = Mensagens.ReturnSuccess;
        }

        public string Mensagem { get; set; }
        public bool TemErros { get; set; }
        public T ObjetoRetorno { get; set; }
    }
}
