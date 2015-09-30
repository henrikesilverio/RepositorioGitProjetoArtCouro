using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.DataBase.EntityConfig.UsuarioConfiguration
{
    public class GrupoPermissaoConfiguration : EntityTypeConfiguration<GrupoPermissao>
    {
        public GrupoPermissaoConfiguration()
        {
            ToTable("GrupoPermissao");

            Property(x => x.GrupoPermissaoId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.GrupoPermissaoCodigo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.GrupoPermissaoNome)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar");

            HasMany(x => x.Permissoes);
        }
    }
}
