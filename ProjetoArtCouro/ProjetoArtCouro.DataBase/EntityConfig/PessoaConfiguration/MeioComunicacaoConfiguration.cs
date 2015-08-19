using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.DataBase.EntityConfig.PessoaConfiguration
{
    public class MeioComunicacaoConfiguration : EntityTypeConfiguration<MeioComunicacao>
    {
        public MeioComunicacaoConfiguration()
        {
            ToTable("MeioComunicacao");

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

            Property(x => x.TipoComunicacao)
                .IsRequired()
                .HasColumnType("int");

            Property(x => x.Principal)
                .IsRequired()
                .HasColumnType("bit");

            //Relacionamento 1 pra N obrigatorio
            HasRequired(x => x.Pessoa)
                .WithMany(x => x.MeiosComunicacao)
                .HasForeignKey(x => x.PessoaId);
        }
    }
}
