using System.Collections.Generic;
using AutoMapper;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Model.Models.Cliente;
using ProjetoArtCouro.Model.Models.Usuario;

namespace ProjetoArtCouro.Api.AutoMapper
{
    public class ModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
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

            Mapper.CreateMap<ClienteModel, PessoaFisica>()
                .ForMember(d => d.EstadoCivil, m => m.MapFrom(s => s));

            Mapper.CreateMap<ClienteModel, Pessoa>()
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
                        Estado = new Estado{EstadoCodigo = s.Endereco.UFId??0},
                        Principal = true
                    }
                }));

            Mapper.CreateMap<ClienteModel, EstadoCivil>()
                .ForMember(d => d.EstadoCivilId, m => m.Ignore())
                .ForMember(d => d.EstadoCivilNome, m => m.Ignore())
                .ForMember(d => d.EstadoCivilCodigo, m => m.MapFrom(s => s.EstadoCivilId));

        }
    }
}