using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.DataBase.EntityConfig.UsuarioConfiguration
{
    public class UsuarioConfiguration : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfiguration()
        {
            ToTable("Usuario");

            Property(x => x.UsuarioId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(state => state.UsuarioCodigo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.UsuarioNome)
                .HasMaxLength(60)
                .IsRequired();

            Property(x => x.Senha)
                .HasMaxLength(32)
                .IsRequired()
                .IsFixedLength();

            Property(x => x.Ativo)
                .IsRequired()
                .HasColumnType("bit");

            //Relação n pra n Usuario, Permissão
            HasMany(x => x.Permissoes)
                .WithMany(x => x.Usuarios)
                .Map(m =>
                {
                    m.MapLeftKey("UsuarioId");
                    m.MapRightKey("PermissaoId");
                    m.ToTable("UsuarioPermissao");
                });
        }
    }
}
