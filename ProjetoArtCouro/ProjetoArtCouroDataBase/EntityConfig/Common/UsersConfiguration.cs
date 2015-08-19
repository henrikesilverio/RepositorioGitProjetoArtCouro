using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouroDomain.Registers;

namespace ProjetoArtCouroDataBase.EntityConfig.Common
{
    public class UsersConfiguration : EntityTypeConfiguration<Users>
    {
        public UsersConfiguration()
        {
            HasKey(user => user.Id);
        }
    }
}
