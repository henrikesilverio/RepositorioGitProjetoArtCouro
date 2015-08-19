using System.Collections.Generic;
using AutoMapper;
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
                .ForMember(d => d.MeiosComunicacao, m => m.MapFrom(s => s.MeioComunicacao))
                .ForMember(d => d.Enderecos, m => m.MapFrom(s => s.Endereco));

            Mapper.CreateMap<ClienteModel, EstadoCivil>()
                .ForMember(d => d.Codigo, m => m.MapFrom(s => s.EstadoCivilId));

            Mapper.CreateMap<MeioComunicacaoModel, ICollection<MeioComunicacao>>()
                .ForMember(d => d, m => m.MapFrom(s => new List<MeioComunicacao>
                {
                    new MeioComunicacao
                    {
                        Nome = s.Celular,
                        Codigo = s.CelularId ?? 0
                    },
                    new MeioComunicacao
                    {
                        Nome = s.Email,
                        Codigo = s.EmailId ?? 0
                    },
                    new MeioComunicacao
                    {
                        Nome = s.Telefone,
                        Codigo = s.TelefoneId ?? 0
                    }
                }));

            Mapper.CreateMap<EnderecoModel, Endereco>()
                .ForMember(d => d.EnderecoCodigo, m => m.MapFrom(s => s.EnderecoId))
                .ForMember(d => d.Estado, m => m.MapFrom(s => new Estado {Codigo = s.UFId ?? 0}));

            Mapper.CreateMap<EnderecoModel, ICollection<Endereco>>()
                .ForMember(d => d, m => m.MapFrom(s => new List<Endereco>
                {
                    Mapper.Map<Endereco>(s)
                }));
        }
    }
}