using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.DataBase.EntityConfig.PessoaConfiguration
{
    public class PessoaConfiguration : EntityTypeConfiguration<Pessoa>
    {
        public PessoaConfiguration()
        {
            ToTable("Pessoa");

            Property(person => person.PessoaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(state => state.PessoaCodigo)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_PESSOA_CODIGO", 1) { IsUnique = true }));

            Property(person => person.Nome)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("varchar");

            //Relacionamento 1 : N
            HasMany(x => x.MeiosComunicacao)
                .WithRequired(x => x.Pessoa)
                .HasForeignKey(x => x.PessoaId);

            //Relacionamento 1 : N
            HasMany(x => x.Enderecos)
                .WithRequired(x => x.Pessoa)
                .HasForeignKey(x => x.PessoaId);

            //Relacionamento 1 : 0 ou 1
            HasOptional(x => x.PessoaFisica)
                .WithRequired(x => x.Pessoa);

            //Relacionamento 1 : 0 ou 1
            HasOptional(x => x.PessoaJuridica)
                .WithRequired(x => x.Pessoa);

            //Relacionamento N : N
            HasMany(x => x.Papeis)
                .WithMany(x => x.Pessoas)
                .Map(m =>
                {
                    m.MapLeftKey("PessoaId");
                    m.MapRightKey("Id");
                    m.ToTable("PessoaPapel");
                });
        }
    }
}
