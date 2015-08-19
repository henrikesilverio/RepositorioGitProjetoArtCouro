using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouroDomain.GeneralUse;

namespace ProjetoArtCouroDataBase.EntityConfig.Common
{
    public class MaritalStatusConfiguration : EntityTypeConfiguration<MaritalStatus>
    {
        public MaritalStatusConfiguration()
        {
            HasKey(maritalStatus => maritalStatus.Id);
        }
    }
}
