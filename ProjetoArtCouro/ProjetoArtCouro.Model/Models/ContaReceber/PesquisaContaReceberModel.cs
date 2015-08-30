using System;
using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.ContaReceber
{
    public class PesquisaContaReceberModel
    {
        [Display(Name = "SaleCode", ResourceType = typeof(Mensagens))]
        public int? CodigoVenda { get; set; }

        [Display(Name = "ClientCode", ResourceType = typeof(Mensagens))]
        public int? CodigoCliente { get; set; }

        [Display(Name = "IssuanceDate", ResourceType = typeof(Mensagens))]
        public DateTime? DataEmissao { get; set; }

        [Display(Name = "DueDate", ResourceType = typeof(Mensagens))]
        public DateTime? DataVencimento { get; set; }

        [Display(Name = "ClientName", ResourceType = typeof(Mensagens))]
        public string NomeCliente { get; set; }

        [Display(Name = "CPFCNPJ", ResourceType = typeof(Mensagens))]
        public string CPFCNPJ { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Mensagens))]
        public int? StatusId { get; set; }
    }
}
