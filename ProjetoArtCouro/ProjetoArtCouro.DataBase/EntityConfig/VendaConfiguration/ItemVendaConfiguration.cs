using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Vendas;

namespace ProjetoArtCouro.DataBase.EntityConfig.VendaConfiguration
{
    public class ItemVendaConfiguration : EntityTypeConfiguration<ItemVenda>
    {
        public ItemVendaConfiguration()
        {
            ToTable("ItemVenda");

            Property(x => x.ItemVendaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.ItemVendaCodigo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.ProdutoCodigo)
                .IsRequired()
                .HasColumnType("int");

            Property(x => x.ProdutoNome)
                .IsRequired()
                .HasColumnType("varchar");

            Property(x => x.Quantidade)
                .IsRequired()
                .HasColumnType("int");

            Property(x => x.PrecoVenda)
                .IsRequired()
                .HasColumnType("decimal");

            Property(x => x.ValorBruto)
                .IsRequired()
                .HasColumnType("decimal");

            Property(x => x.ValorDesconto)
                .HasColumnType("decimal");

            Property(x => x.ValorLiquido)
                .IsRequired()
                .HasColumnType("decimal");
        }
    }
}
