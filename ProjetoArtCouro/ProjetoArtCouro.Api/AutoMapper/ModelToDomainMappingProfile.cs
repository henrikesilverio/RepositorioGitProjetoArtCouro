using System;
using System.Collections.Generic;
using AutoMapper;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Domain.Models.Compras;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pagamentos;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Domain.Models.Produtos;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Domain.Models.Vendas;
using ProjetoArtCouro.Model.Models.Cliente;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Compra;
using ProjetoArtCouro.Model.Models.CondicaoPagamento;
using ProjetoArtCouro.Model.Models.FormaPagamento;
using ProjetoArtCouro.Model.Models.Fornecedor;
using ProjetoArtCouro.Model.Models.Funcionario;
using ProjetoArtCouro.Model.Models.Produto;
using ProjetoArtCouro.Model.Models.Usuario;
using ProjetoArtCouro.Model.Models.Venda;

namespace ProjetoArtCouro.Api.AutoMapper
{
    public class ModelToDomainMappingProfile : Profile
    {
        public string Profile
        {
            get { return "ModelToDomainMappingProfile"; }
        }

        //Configuração para auto mapeamento de classes
        protected override void Configure()
        {
            Mapper.CreateMap<UsuarioModel, Usuario>()
                .ForMember(d => d.GrupoPermissao,
                    m => m.MapFrom(s => new GrupoPermissao {GrupoPermissaoCodigo = s.GrupoId.Value}));

            Mapper.CreateMap<PermissaoModel, Permissao>()
                .ForMember(d => d.PermissaoCodigo, m => m.MapFrom(s => s.Codigo))
                .ForMember(d => d.PermissaoNome, m => m.MapFrom(s => s.Nome));

            Mapper.CreateMap<GrupoModel, GrupoPermissao>()
                .ForMember(d => d.GrupoPermissaoCodigo, m => m.MapFrom(s => s.GrupoCodigo))
                .ForMember(d => d.GrupoPermissaoNome, m => m.MapFrom(s => s.GrupoNome))
                .ForMember(d => d.Permissoes, m => m.MapFrom(s => s.Permissoes));

            Mapper.CreateMap<PessoaModel, PessoaFisica>()
                .ForMember(d => d.EstadoCivil, m => m.MapFrom(s => s))
                .Include<ClienteModel, PessoaFisica>()
                .Include<FuncionarioModel, PessoaFisica>()
                .Include<FornecedorModel, PessoaFisica>();

            Mapper.CreateMap<PessoaModel, PessoaJuridica>()
                .Include<ClienteModel, PessoaJuridica>()
                .Include<FuncionarioModel, PessoaJuridica>()
                .Include<FornecedorModel, PessoaJuridica>();

            Mapper.CreateMap<PessoaModel, Pessoa>()
                .ForMember(d => d.PessoaCodigo, m => m.MapFrom(s => s.Codigo))
                .ForMember(d => d.Nome, m => m.MapFrom(s => s.EPessoaFisica ? s.Nome : s.RazaoSocial))
                .ForMember(d => d.MeiosComunicacao, m => m.MapFrom(s => new List<MeioComunicacao>
                {
                    new MeioComunicacao
                    {
                        MeioComunicacaoCodigo = s.MeioComunicacao.TelefoneId ?? 0,
                        MeioComunicacaoNome = s.MeioComunicacao.Telefone,
                        TipoComunicacao = TipoComunicacaoEnum.Telefone,
                        Principal = true
                    },
                    new MeioComunicacao
                    {
                        MeioComunicacaoCodigo = s.MeioComunicacao.CelularId ?? 0,
                        MeioComunicacaoNome = s.MeioComunicacao.Celular,
                        TipoComunicacao = TipoComunicacaoEnum.Celular,
                        Principal = true
                    },
                    new MeioComunicacao
                    {
                        MeioComunicacaoCodigo = s.MeioComunicacao.EmailId ?? 0,
                        MeioComunicacaoNome = s.MeioComunicacao.Email,
                        TipoComunicacao = TipoComunicacaoEnum.Email,
                        Principal = true
                    }
                }))
                .ForMember(d => d.Enderecos, m => m.MapFrom(s => new List<Endereco>
                {
                    new Endereco
                    {
                        EnderecoCodigo = s.Endereco.EnderecoId ?? 0,
                        Logradouro = s.Endereco.Logradouro,
                        Numero = s.Endereco.Numero,
                        Bairro = s.Endereco.Bairro,
                        Complemento = s.Endereco.Complemento,
                        Cidade = s.Endereco.Cidade,
                        CEP = s.Endereco.Cep,
                        Estado = new Estado {EstadoCodigo = s.Endereco.UFId ?? 0},
                        Principal = true
                    }
                }))
                .Include<ClienteModel, Pessoa>()
                .Include<FuncionarioModel, Pessoa>()
                .Include<FornecedorModel, Pessoa>();

            Mapper.CreateMap<PessoaModel, EstadoCivil>()
                .ForMember(d => d.EstadoCivilId, m => m.Ignore())
                .ForMember(d => d.EstadoCivilNome, m => m.Ignore())
                .ForMember(d => d.EstadoCivilCodigo, m => m.MapFrom(s => s.EstadoCivilId))
                .Include<ClienteModel, EstadoCivil>()
                .Include<FuncionarioModel, EstadoCivil>()
                .Include<FornecedorModel, EstadoCivil>();

            Mapper.CreateMap<ProdutoModel, Produto>()
                .ForMember(d => d.ProdutoId, m => m.Ignore())
                .ForMember(d => d.ProdutoNome, m => m.MapFrom(s => s.Descricao))
                .ForMember(d => d.PrecoCusto, m => m.MapFrom(s => decimal.Parse(s.PrecoCusto)))
                .ForMember(d => d.PrecoVenda, m => m.MapFrom(s => decimal.Parse(s.PrecoVenda)))
                .ForMember(d => d.Unidade, m => m.MapFrom(s => new Unidade {UnidadeCodigo = s.UnidadeId}));

            Mapper.CreateMap<CondicaoPagamentoModel, CondicaoPagamento>();

            Mapper.CreateMap<FormaPagamentoModel, FormaPagamento>();

            Mapper.CreateMap<VendaModel, Venda>()
                .ForMember(d => d.Cliente, m => m.MapFrom(s => new Pessoa {PessoaCodigo = s.ClienteId ?? 0}))
                .ForMember(d => d.CondicaoPagamento,
                    m => m.MapFrom(s => new CondicaoPagamento {CondicaoPagamentoCodigo = s.CondicaoPagamentoId ?? 0}))
                .ForMember(d => d.FormaPagamento,
                    m => m.MapFrom(s => new FormaPagamento {FormaPagamentoCodigo = s.FormaPagamentoId ?? 0}))
                .ForMember(d => d.DataCadastro, m => m.MapFrom(s => s.DataCadastro.ToDateTime()))
                .ForMember(d => d.ItensVenda, m => m.MapFrom(s => s.ItemVendaModel))
                .ForMember(d => d.StatusVenda, m => m.MapFrom(s => Enum.Parse(typeof (StatusVendaEnum), s.Status)))
                .ForMember(d => d.VendaCodigo, m => m.MapFrom(s => s.CodigoVenda ?? 0))
                .ForMember(d => d.ValorTotalBruto, m => m.MapFrom(s => s.ValorTotalBruto.ToDecimal()))
                .ForMember(d => d.ValorTotalDesconto, m => m.MapFrom(s => s.ValorTotalDesconto.ToDecimal()))
                .ForMember(d => d.ValorTotalLiquido, m => m.MapFrom(s => s.ValorTotalLiquido.ToDecimal()));

            Mapper.CreateMap<ItemVendaModel, ItemVenda>()
                .ForMember(d => d.ProdutoCodigo, m => m.MapFrom(s => s.Codigo))
                .ForMember(d => d.ProdutoNome, m => m.MapFrom(s => s.Descricao))
                .ForMember(d => d.PrecoVenda, m => m.MapFrom(s => s.PrecoVenda.ToDecimal()))
                .ForMember(d => d.ValorBruto, m => m.MapFrom(s => s.ValorBruto.ToDecimal()))
                .ForMember(d => d.ValorDesconto, m => m.MapFrom(s => s.ValorDesconto.ToDecimal()))
                .ForMember(d => d.ValorLiquido, m => m.MapFrom(s => s.ValorLiquido.ToDecimal()));

            Mapper.CreateMap<CompraModel, Compra>()
               .ForMember(d => d.Fornecedor, m => m.MapFrom(s => new Pessoa { PessoaCodigo = s.FornecedorId ?? 0 }))
               .ForMember(d => d.CondicaoPagamento,
                   m => m.MapFrom(s => new CondicaoPagamento { CondicaoPagamentoCodigo = s.CondicaoPagamentoId ?? 0 }))
               .ForMember(d => d.FormaPagamento,
                   m => m.MapFrom(s => new FormaPagamento { FormaPagamentoCodigo = s.FormaPagamentoId ?? 0 }))
               .ForMember(d => d.DataCadastro, m => m.MapFrom(s => s.DataCadastro.ToDateTime()))
               .ForMember(d => d.ItensCompra, m => m.MapFrom(s => s.ItemCompraModel))
               .ForMember(d => d.StatusCompra, m => m.MapFrom(s => Enum.Parse(typeof(StatusCompraEnum), s.StatusCompra)))
               .ForMember(d => d.CompraCodigo, m => m.MapFrom(s => s.CodigoCompra ?? 0))
               .ForMember(d => d.ValorTotalBruto, m => m.MapFrom(s => s.ValorTotalBruto.ToDecimal()))
               .ForMember(d => d.ValorTotalFrete, m => m.MapFrom(s => s.ValorTotalFrete.ToDecimal()))
               .ForMember(d => d.ValorTotalLiquido, m => m.MapFrom(s => s.ValorTotalLiquido.ToDecimal()));

            Mapper.CreateMap<ItemCompraModel, ItemCompra>()
                .ForMember(d => d.ProdutoCodigo, m => m.MapFrom(s => s.Codigo))
                .ForMember(d => d.ProdutoNome, m => m.MapFrom(s => s.Descricao))
                .ForMember(d => d.PrecoVenda, m => m.MapFrom(s => s.PrecoVenda.ToDecimal()))
                .ForMember(d => d.ValorBruto, m => m.MapFrom(s => s.ValorBruto.ToDecimal()))
                .ForMember(d => d.ValorLiquido, m => m.MapFrom(s => s.ValorLiquido.ToDecimal()));
        }
    }
}