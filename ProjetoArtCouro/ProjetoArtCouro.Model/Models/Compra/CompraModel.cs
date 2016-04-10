using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resources.Resources;

namespace ProjetoArtCouro.Model.Models.Compra
{
    public class CompraModel
    {
        [Display(Name = "CodeBuy", ResourceType = typeof(Mensagens))]
        public int? CodigoCompra { get; set; }

        [Display(Name = "RegistrationDate", ResourceType = typeof(Mensagens))]
        public string DataCadastro { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Mensagens))]
        public string StatusCompra { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Providers", ResourceType = typeof(Mensagens))]
        public int? FornecedorId { get; set; }

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

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "TotalValueShipping", ResourceType = typeof(Mensagens))]
        public string ValorTotalFrete { get; set; }

        [Display(Name = "TotalValueLiquid", ResourceType = typeof(Mensagens))]
        public string ValorTotalLiquido { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Quantity", ResourceType = typeof(Mensagens))]
        public int? Quantidade { get; set; }

        [Required(ErrorMessageResourceType = typeof(Erros), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Products", ResourceType = typeof(Mensagens))]
        public int? ProdutoId { get; set; }

        public string NomeFornecedor { get; set; }

        public string CPFCNPJ { get; set; }

        public List<ItemCompraModel> ItemCompraModel { get; set; }
    }
}
