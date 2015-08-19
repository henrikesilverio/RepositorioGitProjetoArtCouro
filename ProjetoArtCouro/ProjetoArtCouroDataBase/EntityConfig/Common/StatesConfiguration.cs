using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouroDomain.GeneralUse;

namespace ProjetoArtCouroDataBase.EntityConfig.Common
{
    public class StatesConfiguration : EntityTypeConfiguration<States>
    {
        public StatesConfiguration()
        {
            HasKey(state => state.Id);
        }
    }
}
