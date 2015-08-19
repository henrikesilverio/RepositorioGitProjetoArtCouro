using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.DataBase.EntityConfig.PessoaConfiguration
{
    public class PessoaFisicaConfiguration : EntityTypeConfiguration<PessoaFisica>
    {
        public PessoaFisicaConfiguration()
        {
            ToTable("PessoaFisica");

            Property(x => x.PessoaFisicaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.CPF)
                .IsRequired()
                .HasColumnType("varchar")
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CPF", 1) { IsUnique = true }));

            Property(x => x.RG)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(15);

            Property(x => x.Sexo)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(10);

            //Relação 1 pra 0 ou 1 pessoa, pessoa fisica
            HasRequired(x => x.Pessoa)
                .WithOptional(x => x.PessoaFisica);

            //Relação 1 pra 0 ou 1 pessoa fisica, usuario
            //HasOptional(x => x.Usuario)
            //    .WithRequired(x => x.PessoaFisica);

            //Relação 1 pra 0 ou 1 estado civil, pessoa fisica
            HasRequired(x => x.EstadoCivil)
                .WithOptional(x => x.PessoaFisica);
        }
    }
}
