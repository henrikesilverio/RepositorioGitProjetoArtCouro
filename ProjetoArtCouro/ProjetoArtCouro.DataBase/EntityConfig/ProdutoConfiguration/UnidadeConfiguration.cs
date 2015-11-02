using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Produtos;

namespace ProjetoArtCouro.DataBase.EntityConfig.ProdutoConfiguration
{
    public class UnidadeConfiguration : EntityTypeConfiguration<Unidade>
    {
        public UnidadeConfiguration()
        {
            ToTable("Unidade");

            Property(x => x.UnidadeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.UnidadeCodigo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.UnidadeNome)
               .IsRequired()
               .HasMaxLength(30)
               .HasColumnType("varchar");

            //Relacionamento 1 : N
            HasMany(x => x.Produto)
                .WithRequired(x => x.Unidade);
        }
    }
}
