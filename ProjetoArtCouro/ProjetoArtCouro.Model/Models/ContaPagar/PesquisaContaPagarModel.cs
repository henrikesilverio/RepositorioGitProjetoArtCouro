using System;
using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.ContaPagar
{
    public class PesquisaContaPagarModel
    {
        [Display(Name = "CodeBuy", ResourceType = typeof(Mensagens))]
        public int? CodigoCompra { get; set; }

        [Display(Name = "ProviderCode", ResourceType = typeof(Mensagens))]
        public int? CodigoFornecedor { get; set; }

        [Display(Name = "IssuanceDate", ResourceType = typeof(Mensagens))]
        public DateTime? DataEmissao { get; set; }

        [Display(Name = "DueDate", ResourceType = typeof(Mensagens))]
        public DateTime? DataVencimento { get; set; }

        [Display(Name = "ProviderName", ResourceType = typeof(Mensagens))]
        public string NomeFornecedor { get; set; }

        [Display(Name = "CPFCNPJ", ResourceType = typeof(Mensagens))]
        public string CPFCNPJ { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Mensagens))]
        public int? StatusId { get; set; }
    }
}
