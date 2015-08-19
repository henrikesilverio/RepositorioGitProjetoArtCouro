using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.DataBase.EntityConfig.PessoaConfiguration
{
    public class PapelConfiguration : EntityTypeConfiguration<Papel>
    {
        public PapelConfiguration()
        {
            ToTable("Papel");

            Property(x => x.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Codigo)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CODIGO", 1) { IsUnique = true }));

            Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnType("varchar");

            HasMany(x => x.Pessoas);
        }
    }
}
