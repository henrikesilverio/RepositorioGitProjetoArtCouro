using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Produtos;

namespace ProjetoArtCouro.DataBase.EntityConfig.ProdutoConfiguration
{
    public class ProdutoConfiguration : EntityTypeConfiguration<Produto>
    {
        public ProdutoConfiguration()
        {
            ToTable("Produto");

            Property(x => x.ProdutoId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.ProdutoCodigo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.ProdutoNome)
               .IsRequired()
               .HasMaxLength(200)
               .HasColumnType("varchar");

            Property(x => x.PrecoVenda)
                .IsRequired()
                .HasColumnType("decimal");

            Property(x => x.PrecoCusto)
                .IsRequired()
                .HasColumnType("decimal");
        }
    }
}
