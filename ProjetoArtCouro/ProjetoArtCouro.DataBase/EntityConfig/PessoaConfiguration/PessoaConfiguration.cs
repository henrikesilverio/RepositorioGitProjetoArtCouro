using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.DataBase.EntityConfig.PessoaConfiguration
{
    public class PessoaConfiguration : EntityTypeConfiguration<Pessoa>
    {
        public PessoaConfiguration()
        {
            ToTable("Pessoa");

            Property(x => x.PessoaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.PessoaCodigo)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("varchar");

            //Relacionamento 1 : N
            HasMany(x => x.Enderecos)
                .WithRequired(x => x.Pessoa)
                .WillCascadeOnDelete();

            //Relacionamento 1 : N
            HasMany(x => x.MeiosComunicacao)
                .WithRequired(x => x.Pessoa)
                .WillCascadeOnDelete();

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
                    m.MapRightKey("PapelId");
                    m.ToTable("PessoaPapel");
                });
        }
    }
}
