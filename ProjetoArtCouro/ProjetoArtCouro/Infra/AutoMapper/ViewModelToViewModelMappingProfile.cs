using AutoMapper;
using ProjetoArtCouro.Web.Models.Registers;

namespace ProjetoArtCouro.Web.Infra.AutoMapper
{
    public class ViewModelToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToViewModelMappingProfile"; }
        }

        //Configuração de mapeamento do viewModel para viewModel
        protected override void Configure()
        {
            Mapper.CreateMap<PersonModelView, AddressModelView>();
            Mapper.CreateMap<PersonModelView, PhysicalPersonModelView>();
            Mapper.CreateMap<PersonModelView, LegalPersonModelView>();
        }
    }
}