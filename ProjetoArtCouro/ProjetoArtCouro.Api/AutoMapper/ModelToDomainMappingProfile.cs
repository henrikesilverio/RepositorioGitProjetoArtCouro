using System.Collections.Generic;
using AutoMapper;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Domain.Models.Produtos;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Model.Models.Cliente;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Produto;
using ProjetoArtCouro.Model.Models.Usuario;

namespace ProjetoArtCouro.Api.AutoMapper
{
    public class ModelToDomainMappingProfile : Profile
    {
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
                .Include<ClienteModel, PessoaFisica>();

            Mapper.CreateMap<PessoaModel, PessoaJuridica>()
                .Include<ClienteModel, PessoaJuridica>();

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
                .Include<ClienteModel, Pessoa>();

            Mapper.CreateMap<PessoaModel, EstadoCivil>()
                .ForMember(d => d.EstadoCivilId, m => m.Ignore())
                .ForMember(d => d.EstadoCivilNome, m => m.Ignore())
                .ForMember(d => d.EstadoCivilCodigo, m => m.MapFrom(s => s.EstadoCivilId))
                .Include<ClienteModel, EstadoCivil>();

            Mapper.CreateMap<ProdutoModel, Produto>()
                .ForMember(d => d.ProdutoId, m => m.Ignore())
                .ForMember(d => d.ProdutoNome, m => m.MapFrom(s => s.Descricao))
                .ForMember(d => d.PrecoCusto, m => m.MapFrom(s => decimal.Parse(s.PrecoCusto)))
                .ForMember(d => d.PrecoVenda, m => m.MapFrom(s => decimal.Parse(s.PrecoVenda)))
                .ForMember(d => d.Unidade, m => m.MapFrom(s => new Unidade {UnidadeCodigo = s.UnidadeId}));
        }
    }
}