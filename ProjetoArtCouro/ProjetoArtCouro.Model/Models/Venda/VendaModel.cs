using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Model.Models.Produto;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Venda
{
    public class VendaModel
    {
        [Display(Name = "SaleCode", ResourceType = typeof(Mensagens))]
        public int? CodigoVenda { get; set; }

        [Display(Name = "RegistrationDate", ResourceType = typeof(Mensagens))]
        public DateTime DataCadastro { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Mensagens))]
        public string Status { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Customers", ResourceType = typeof (Mensagens))]
        public int? ClienteId { get; set; }

        [Display(Name = "Employees", ResourceType = typeof(Mensagens))]
        public int? FuncionarioId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "FormsPayment", ResourceType = typeof(Mensagens))]
        public int? FormaPagamentoId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "ConditionsPayments", ResourceType = typeof(Mensagens))]
        public int? CondicaoPagamentoId { get; set; }

        [Display(Name = "TotalCrudeValue", ResourceType = typeof(Mensagens))]
        public string ValorTotalBruto { get; set; }

        [Display(Name = "TotalValueDiscount", ResourceType = typeof(Mensagens))]
        public string ValorTotalDesconto { get; set; }

        [Display(Name = "TotalValueLiquid", ResourceType = typeof(Mensagens))]
        public string ValorTotalLiquido { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Quantity", ResourceType = typeof(Mensagens))]
        public string Quantidade { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Products", ResourceType = typeof(Mensagens))]
        public int? ProdutoId { get; set; }

        [Display(Name = "ValueDiscount", ResourceType = typeof(Mensagens))]
        public string ValorDesconto { get; set; }

        public string NomeCliente { get; set; }

        public string CPFCNPJ { get; set; }

        public List<ProdutoModel> ProdutoModel { get; set; }
    }
}
