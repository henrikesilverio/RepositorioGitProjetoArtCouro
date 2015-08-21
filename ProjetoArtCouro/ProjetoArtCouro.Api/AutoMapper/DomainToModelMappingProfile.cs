using AutoMapper;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Model.Models.Common;

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
            Mapper.CreateMap<Estado, LookupModel>();
            Mapper.CreateMap<EstadoCivil, LookupModel>();
        }
    }
}