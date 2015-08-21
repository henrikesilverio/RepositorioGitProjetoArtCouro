using System.Collections.Generic;
using AutoMapper;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Model.Models.Cliente;
using ProjetoArtCouro.Model.Models.Usuarios;

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
            Mapper.CreateMap<PermissaoModel, Permissao>();

            Mapper.CreateMap<ClienteModel, PessoaFisica>()
                .ForMember(d => d.Pessoa, m => m.MapFrom(s => s))
                .ForMember(d => d.EstadoCivil, m => m.MapFrom(s => s));

            Mapper.CreateMap<ClienteModel, Pessoa>()
                .ForMember(d => d.MeiosComunicacao, m => m.MapFrom(s => new List<MeioComunicacao>
                {
                    new MeioComunicacao
                    {
                        Codigo = s.MeioComunicacao.TelefoneId ?? 0,
                        Nome = s.MeioComunicacao.Telefone,
                        TipoComunicacao = TipoComunicacaoEnum.Telefone,
                        Principal = true
                    },
                    new MeioComunicacao
                    {
                        Codigo = s.MeioComunicacao.CelularId ?? 0,
                        Nome = s.MeioComunicacao.Celular,
                        TipoComunicacao = TipoComunicacaoEnum.Celular,
                        Principal = true
                    },
                    new MeioComunicacao
                    {
                        Codigo = s.MeioComunicacao.EmailId ?? 0,
                        Nome = s.MeioComunicacao.Email,
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
                        Estado = new Estado{Codigo = s.Endereco.UFId??0},
                        Principal = true
                    }
                }));

            Mapper.CreateMap<ClienteModel, EstadoCivil>()
                .ForMember(d => d.Codigo, m => m.MapFrom(s => s.EstadoCivilId));

        }
    }
}