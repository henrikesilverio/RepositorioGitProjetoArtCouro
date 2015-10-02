using System.Collections.Generic;
using AutoMapper;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Model.Models.Cliente;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Usuario;

namespace ProjetoArtCouro.Api.AutoMapper
{
    public class DomainToModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToModelMappingProfile"; }
        }

        //Configuração de mapeamento do viewModel para o dominio
        protected override void Configure()
        {
            Mapper.CreateMap<Usuario, UsuarioModel>()
                .ForMember(d => d.Permissoes, m => m.MapFrom(s => s.Permissoes))
                .ForMember(d => d.Senha, m => m.Ignore());

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

            Mapper.CreateMap<PessoaFisica, ClienteModel>()
                .ForMember(d => d.Codigo, m => m.MapFrom(s => s.Pessoa.PessoaCodigo))
                .ForMember(d => d.Nome, m => m.MapFrom(s => s.Pessoa.Nome))
                .ForMember(d => d.EstadoCivilId, m => m.MapFrom(s => s.EstadoCivil.EstadoCivilCodigo))
                .ForMember(d => d.EPessoaFisica, m => m.UseValue(true))
                .ForMember(d => d.Endereco, m => m.MapFrom(s => s.Pessoa.Enderecos))
                .ForMember(d => d.MeioComunicacao, m => m.MapFrom(s => s.Pessoa.MeiosComunicacao));

            Mapper.CreateMap<ICollection<MeioComunicacao>, MeioComunicacaoModel>()
                .ConvertUsing<MeioComunicacaoConverter>();

            Mapper.CreateMap<ICollection<Endereco>, EnderecoModel>()
                .ConvertUsing<EnderecoConverter>();
        }
    }
}