using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.DataBase.EntityConfig.PessoaConfiguration
{
    public class PessoaJuridicaConfiguration : EntityTypeConfiguration<PessoaJuridica>
    {
        public PessoaJuridicaConfiguration()
        {
            ToTable("PessoaJuridica");

            Property(x => x.PessoaJuridicaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.CNPJ)
                .IsRequired()
                .HasColumnType("varchar")
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CNPJ", 1) { IsUnique = true }));

            Property(x => x.Contato)
                .IsOptional()
                .HasMaxLength(100)
                .HasColumnType("varchar");

            //Relação 1 pra 0 1 pessoa, pessoa juridica
            HasRequired(x => x.Pessoa)
                .WithOptional(x => x.PessoaJuridica);
        }
    }
}
