using AutoMapper;

namespace ProjetoArtCouro.Web.Infra.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            //Definindo e Iniciando os mapeamentos
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainMappingProfile>();
                x.AddProfile<ViewModelToViewModelMappingProfile>();
            });
        }
    }
}