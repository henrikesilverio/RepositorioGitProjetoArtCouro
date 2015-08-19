using AutoMapper;

namespace ProjetoArtCouro.Api.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            //Definindo e Iniciando os mapeamentos
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToModelMappingProfile>();
                x.AddProfile<ModelToDomainMappingProfile>();
                x.AddProfile<ModelToModelMappingProfile>();
            });
        }
    }
}