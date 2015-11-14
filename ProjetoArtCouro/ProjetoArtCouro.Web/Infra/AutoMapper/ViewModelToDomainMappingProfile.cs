using AutoMapper;

namespace ProjetoArtCouro.Web.Infra.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public string Profile
        {
            get { return "DomainToViewModelMappings"; }
        }

        //Configuração para auto mapeamento de classes
        protected override void Configure()
        {
            //Criar
        }
    }
}