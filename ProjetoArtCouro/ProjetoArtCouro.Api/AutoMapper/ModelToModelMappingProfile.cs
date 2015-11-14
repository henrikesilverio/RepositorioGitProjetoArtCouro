using AutoMapper;

namespace ProjetoArtCouro.Api.AutoMapper
{
    public class ModelToModelMappingProfile : Profile
    {
        public string Profile
        {
            get { return "ModelToModelMappingProfile"; }
        }

        //Configuração de mapeamento do viewModel para viewModel
        protected override void Configure()
        {
            //Criar
        }
    }
}