using AutoMapper;

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
           //Criar
        }
    }
}