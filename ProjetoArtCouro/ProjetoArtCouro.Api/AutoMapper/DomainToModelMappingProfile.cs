using System.Collections.Generic;
using AutoMapper;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Domain.Models.Compras;
using ProjetoArtCouro.Domain.Models.Pagamentos;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Domain.Models.Produtos;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Domain.Models.Vendas;
using ProjetoArtCouro.Model.Models.Cliente;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Compra;
using ProjetoArtCouro.Model.Models.CondicaoPagamento;
using ProjetoArtCouro.Model.Models.ContaPagar;
using ProjetoArtCouro.Model.Models.ContaReceber;
using ProjetoArtCouro.Model.Models.FormaPagamento;
using ProjetoArtCouro.Model.Models.Fornecedor;
using ProjetoArtCouro.Model.Models.Funcionario;
using ProjetoArtCouro.Model.Models.Produto;
using ProjetoArtCouro.Model.Models.Usuario;
using ProjetoArtCouro.Model.Models.Venda;

namespace ProjetoArtCouro.Api.AutoMapper
{
    public class DomainToModelMappingProfile : Profile
    {
        public string Profile
        {
            get { return "DomainToModelMappingProfile"; }
        }

        //Configuração de mapeamento do viewModel para o dominio
        protected override void Configure()
        {
            Mapper.CreateMap<Usuario, UsuarioModel>()
                .ForMember(d => d.Permissoes, m => m.MapFrom(s => s.Permissoes))
                .ForMember(d => d.Senha, m => m.Ignore())
                .ForMember(d => d.GrupoId, m => m.MapFrom(s => s.GrupoPermissao.GrupoPermissaoCodigo));

            Mapper.CreateMap<Permissao, PermissaoModel>()
                .ForMember(d => d.Codigo, m => m.MapFrom(s => s.PermissaoCodigo))
                .ForMember(d => d.Nome, m => m.MapFrom(s => s.PermissaoNome));

            Mapper.CreateMap<GrupoPermissao, GrupoModel>()
                .ForMember(d => d.GrupoCodigo, m => m.MapFrom(s => s.GrupoPermissaoCodigo))
                .ForMember(d => d.GrupoNome, m => m.MapFrom(s => s.GrupoPermissaoNome))
                .ForMember(d => d.Permissoes, m => m.MapFrom(s => s.Permissoes));

            Mapper.CreateMap<Estado, LookupModel>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.EstadoId))
                .ForMember(d => d.Codigo, m => m.MapFrom(s => s.EstadoCodigo))
                .ForMember(d => d.Nome, m => m.MapFrom(s => s.EstadoNome));

            Mapper.CreateMap<EstadoCivil, LookupModel>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.EstadoCivilId))
                .ForMember(d => d.Codigo, m => m.MapFrom(s => s.EstadoCivilCodigo))
                .ForMember(d => d.Nome, m => m.MapFrom(s => s.EstadoCivilNome));

            Mapper.CreateMap<Endereco, EnderecoModel>()
                .ForMember(d => d.EnderecoId, m => m.MapFrom(s => s.EnderecoCodigo))
                .ForMember(d => d.Cep, m => m.MapFrom(s => s.CEP))
                .ForMember(d => d.UFId, m => m.MapFrom(s => s.Estado.EstadoCodigo));

            Mapper.CreateMap<Pessoa, PessoaModel>()
                .ForMember(d => d.Nome, m => m.Ignore())
                .ForMember(d => d.Codigo, m => m.MapFrom(s => s.PessoaCodigo))
                .AfterMap((s, d) =>
                {
                    if (s.PessoaFisica == null)
                    {
                        d.RazaoSocial = s.Nome;
                        d.Nome = null;
                    }
                    else
                    {
                        d.Nome = s.Nome;
                        d.RazaoSocial = null;
                    }
                });

            Mapper.CreateMap<PessoaFisica, PessoaModel>()
                .ForMember(d => d.Codigo, m => m.MapFrom(s => s.Pessoa.PessoaCodigo))
                .ForMember(d => d.Nome, m => m.MapFrom(s => s.Pessoa.Nome))
                .ForMember(d => d.EstadoCivilId, m => m.MapFrom(s => s.EstadoCivil.EstadoCivilCodigo))
                .ForMember(d => d.EPessoaFisica, m => m.UseValue(true))
                .ForMember(d => d.Endereco, m => m.MapFrom(s => s.Pessoa.Enderecos))
                .ForMember(d => d.MeioComunicacao, m => m.MapFrom(s => s.Pessoa.MeiosComunicacao))
                .ForMember(d => d.Enderecos, m => m.MapFrom(s => s.Pessoa.Enderecos))
                .Include<PessoaFisica, ClienteModel>()
                .Include<PessoaFisica, FuncionarioModel>()
                .Include<PessoaFisica, FornecedorModel>();

            Mapper.CreateMap<PessoaFisica, ClienteModel>();
            Mapper.CreateMap<PessoaFisica, FuncionarioModel>();
            Mapper.CreateMap<PessoaFisica, FornecedorModel>();

            Mapper.CreateMap<PessoaJuridica, PessoaModel>()
                .ForMember(d => d.Codigo, m => m.MapFrom(s => s.Pessoa.PessoaCodigo))
                .ForMember(d => d.RazaoSocial, m => m.MapFrom(s => s.Pessoa.Nome))
                .ForMember(d => d.EPessoaFisica, m => m.UseValue(false))
                .ForMember(d => d.Endereco, m => m.MapFrom(s => s.Pessoa.Enderecos))
                .ForMember(d => d.MeioComunicacao, m => m.MapFrom(s => s.Pessoa.MeiosComunicacao))
                .ForMember(d => d.Enderecos, m => m.MapFrom(s => s.Pessoa.Enderecos))
                .Include<PessoaJuridica, ClienteModel>()
                .Include<PessoaJuridica, FuncionarioModel>()
                .Include<PessoaJuridica, FornecedorModel>();

            Mapper.CreateMap<PessoaJuridica, ClienteModel>();
            Mapper.CreateMap<PessoaJuridica, FuncionarioModel>();
            Mapper.CreateMap<PessoaJuridica, FornecedorModel>();

            Mapper.CreateMap<Pessoa, FornecedorModel>()
                .ForMember(d => d.Nome, m => m.Ignore())
                .ForMember(d => d.Codigo, m => m.MapFrom(s => s.PessoaCodigo))
                .AfterMap((d, s) =>
                {
                    if (d.PessoaFisica != null)
                    {
                        s.Nome = d.Nome;
                        s.RazaoSocial = null;
                    }
                    else
                    {
                        s.RazaoSocial = d.Nome;
                        s.Nome = null;
                    }
                });

            Mapper.CreateMap<Produto, ProdutoModel>()
                .ForMember(d => d.Descricao, m => m.MapFrom(s => s.ProdutoNome))
                .ForMember(d => d.PrecoCusto, m => m.MapFrom(s => s.PrecoCusto.ToString("C2")))
                .ForMember(d => d.PrecoVenda, m => m.MapFrom(s => s.PrecoVenda.ToString("C2")))
                .ForMember(d => d.UnidadeId, m => m.MapFrom(s => s.Unidade.UnidadeCodigo))
                .ForMember(d => d.ProdutoCodigo, m => m.MapFrom(s => s.ProdutoCodigo))
                .ForMember(d => d.UnidadeNome, m => m.MapFrom(s => s.Unidade.UnidadeNome));

            Mapper.CreateMap<Unidade, LookupModel>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.UnidadeId))
                .ForMember(d => d.Codigo, m => m.MapFrom(s => s.UnidadeCodigo))
                .ForMember(d => d.Nome, m => m.MapFrom(s => s.UnidadeNome));

            Mapper.CreateMap<CondicaoPagamento, CondicaoPagamentoModel>();

            Mapper.CreateMap<FormaPagamento, FormaPagamentoModel>();

            Mapper.CreateMap<ICollection<MeioComunicacao>, MeioComunicacaoModel>()
                .ConvertUsing<MeioComunicacaoConverter>();

            Mapper.CreateMap<ICollection<Endereco>, EnderecoModel>()
                .ConvertUsing<EnderecoConverter>();

            Mapper.CreateMap<Venda, VendaModel>()
                .ForMember(d => d.CPFCNPJ, m => m.MapFrom(s =>
                    s.Cliente.PessoaFisica == null ? s.Cliente.PessoaJuridica.CNPJ : s.Cliente.PessoaFisica.CPF))
                .ForMember(d => d.ClienteId, m => m.MapFrom(s => s.Cliente.PessoaCodigo))
                .ForMember(d => d.CodigoVenda, m => m.MapFrom(s => s.VendaCodigo))
                .ForMember(d => d.CondicaoPagamentoId, m => m.MapFrom(s => s.CondicaoPagamento.CondicaoPagamentoCodigo))
                .ForMember(d => d.DataCadastro, m => m.MapFrom(s => s.DataCadastro))
                .ForMember(d => d.FormaPagamentoId, m => m.MapFrom(s => s.FormaPagamento.FormaPagamentoCodigo))
                .ForMember(d => d.FuncionarioId, m => m.MapFrom(s => s.Usuario.UsuarioCodigo))
                .ForMember(d => d.ItemVendaModel, m => m.MapFrom(s => s.ItensVenda))
                .ForMember(d => d.NomeCliente, m => m.MapFrom(s => s.Cliente.Nome))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.StatusVenda.ToString()))
                .ForMember(d => d.ValorTotalDesconto, m => m.MapFrom(s => s.ValorTotalDesconto))
                .ForMember(d => d.ValorTotalBruto, m => m.MapFrom(s => s.ValorTotalBruto))
                .ForMember(d => d.ValorTotalLiquido, m => m.MapFrom(s => s.ValorTotalLiquido));

            Mapper.CreateMap<ItemVenda, ItemVendaModel>()
                .ForMember(d => d.Codigo, m => m.MapFrom(s => s.ProdutoCodigo))
                .ForMember(d => d.Descricao, m => m.MapFrom(s => s.ProdutoNome))
                .ForMember(d => d.PrecoVenda, m => m.MapFrom(s => s.PrecoVenda.ToFormatMoney()))
                .ForMember(d => d.ValorBruto, m => m.MapFrom(s => s.ValorBruto.ToFormatMoney()))
                .ForMember(d => d.ValorDesconto, m => m.MapFrom(s => s.ValorDesconto.ToFormatMoney()))
                .ForMember(d => d.ValorLiquido, m => m.MapFrom(s => s.ValorLiquido.ToFormatMoney()));

            Mapper.CreateMap<Compra, CompraModel>()
                .ForMember(d => d.CPFCNPJ, m => m.MapFrom(s =>
                    s.Fornecedor.PessoaFisica == null ? s.Fornecedor.PessoaJuridica.CNPJ : s.Fornecedor.PessoaFisica.CPF))
                .ForMember(d => d.FornecedorId, m => m.MapFrom(s => s.Fornecedor.PessoaCodigo))
                .ForMember(d => d.CodigoCompra, m => m.MapFrom(s => s.CompraCodigo))
                .ForMember(d => d.CondicaoPagamentoId, m => m.MapFrom(s => s.CondicaoPagamento.CondicaoPagamentoCodigo))
                .ForMember(d => d.DataCadastro, m => m.MapFrom(s => s.DataCadastro))
                .ForMember(d => d.FormaPagamentoId, m => m.MapFrom(s => s.FormaPagamento.FormaPagamentoCodigo))
                .ForMember(d => d.FuncionarioId, m => m.MapFrom(s => s.Usuario.UsuarioCodigo))
                .ForMember(d => d.ItemCompraModel, m => m.MapFrom(s => s.ItensCompra))
                .ForMember(d => d.NomeFornecedor, m => m.MapFrom(s => s.Fornecedor.Nome))
                .ForMember(d => d.StatusCompra, m => m.MapFrom(s => s.StatusCompra.ToString()))
                .ForMember(d => d.ValorTotalFrete, m => m.MapFrom(s => s.ValorTotalFrete))
                .ForMember(d => d.ValorTotalBruto, m => m.MapFrom(s => s.ValorTotalBruto))
                .ForMember(d => d.ValorTotalLiquido, m => m.MapFrom(s => s.ValorTotalLiquido));

            Mapper.CreateMap<ItemCompra, ItemCompraModel>()
                .ForMember(d => d.Codigo, m => m.MapFrom(s => s.ProdutoCodigo))
                .ForMember(d => d.Descricao, m => m.MapFrom(s => s.ProdutoNome))
                .ForMember(d => d.PrecoVenda, m => m.MapFrom(s => s.PrecoVenda.ToFormatMoney()))
                .ForMember(d => d.ValorBruto, m => m.MapFrom(s => s.ValorBruto.ToFormatMoney()))
                .ForMember(d => d.ValorLiquido, m => m.MapFrom(s => s.ValorLiquido.ToFormatMoney()));

            Mapper.CreateMap<ContaReceber, ContaReceberModel>()
                .ForMember(d => d.CodigoContaReceber, m => m.MapFrom(s => s.ContaReceberCodigo))
                .ForMember(d => d.CodigoVenda, m => m.MapFrom(s => s.Venda.VendaCodigo))
                .ForMember(d => d.CodigoCliente, m => m.MapFrom(s => s.Venda.Cliente.PessoaCodigo))
                .ForMember(d => d.DataEmissao, m => m.MapFrom(s => s.Venda.DataCadastro))
                .ForMember(d => d.DataVencimento, m => m.MapFrom(s => s.DataVencimento))
                .ForMember(d => d.ValorDocumento, m => m.MapFrom(s => s.ValorDocumento))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.StatusContaReceber.ToString()))
                .ForMember(d => d.StatusId, m => m.MapFrom(s => (int)s.StatusContaReceber))
                .ForMember(d => d.Recebido, m => m.MapFrom(s => s.Recebido))
                .ForMember(d => d.NomeCliente, m => m.MapFrom(s => s.Venda.Cliente.Nome))
                .AfterMap((s, d) =>
                {
                    d.CPFCNPJ = s.Venda.Cliente.PessoaFisica != null
                        ? s.Venda.Cliente.PessoaFisica.CPF
                        : s.Venda.Cliente.PessoaJuridica.CNPJ;
                });

            Mapper.CreateMap<ContaPagar, ContaPagarModel>()
                .ForMember(d => d.CodigoContaPagar, m => m.MapFrom(s => s.ContaPagarCodigo))
                .ForMember(d => d.CodigoCompra, m => m.MapFrom(s => s.Compra.CompraCodigo))
                .ForMember(d => d.CodigoFornecedor, m => m.MapFrom(s => s.Compra.Fornecedor.PessoaCodigo))
                .ForMember(d => d.DataEmissao, m => m.MapFrom(s => s.Compra.DataCadastro))
                .ForMember(d => d.DataVencimento, m => m.MapFrom(s => s.DataVencimento))
                .ForMember(d => d.ValorDocumento, m => m.MapFrom(s => s.ValorDocumento))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.StatusContaPagar.ToString()))
                .ForMember(d => d.StatusId, m => m.MapFrom(s => (int)s.StatusContaPagar))
                .ForMember(d => d.Pago, m => m.MapFrom(s => s.Pago))
                .ForMember(d => d.NomeFornecedor, m => m.MapFrom(s => s.Compra.Fornecedor.Nome))
                .AfterMap((s, d) =>
                {
                    d.CPFCNPJ = s.Compra.Fornecedor.PessoaFisica != null
                        ? s.Compra.Fornecedor.PessoaFisica.CPF
                        : s.Compra.Fornecedor.PessoaJuridica.CNPJ;
                });
        }
    }
}