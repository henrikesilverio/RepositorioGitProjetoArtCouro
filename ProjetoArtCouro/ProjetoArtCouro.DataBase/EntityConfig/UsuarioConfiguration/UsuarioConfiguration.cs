using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
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
               .IsRequired()
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
               .HasColumnAnnotation(
                   IndexAnnotation.AnnotationName,
                   new IndexAnnotation(new IndexAttribute("IX_USUARIO_CODIGO", 1) { IsUnique = true }));

            Property(x => x.UsuarioNome)
                .HasMaxLength(60)
                .IsRequired();

            Property(x => x.Senha)
                .HasMaxLength(32)
                .IsRequired()
                .IsFixedLength();

            //Relação n pra n Usuario, Permissão
            HasMany(x => x.Permissoes)
                .WithMany(x => x.Usuarios)
                .Map(m =>
                {
                    m.MapLeftKey("UsuarioId");
                    m.MapRightKey("Id");
                    m.ToTable("UsuarioPermissao");
                });

            ////Relação 1 pra ou 0 1 Usuario, Pessoa Fisica
            //HasRequired(x => x.PessoaFisica)
            //    .WithOptional(x => x.Usuario);

        }
    }
}
