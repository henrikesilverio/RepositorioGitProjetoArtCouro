using AutoMapper;

namespace ProjetoArtCouro.Web.Infra.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public string Profile
        {
            get { return "ViewModelToDomainMappings"; }
        }

        //Configuração de mapeamento do viewModel para o dominio
        protected override void Configure()
        {
           //Criar
        }
    }
}