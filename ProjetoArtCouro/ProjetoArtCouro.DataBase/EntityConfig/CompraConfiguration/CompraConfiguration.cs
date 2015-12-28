using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Compras;

namespace ProjetoArtCouro.DataBase.EntityConfig.CompraConfiguration
{
    public class CompraConfiguration : EntityTypeConfiguration<Compra>
    {
        public CompraConfiguration()
        {
            ToTable("Compra");

            Property(x => x.CompraId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.CompraCodigo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.DataCadastro)
                .IsRequired()
                .HasColumnType("datetime2");

            Property(x => x.StatusCompra)
                .IsRequired()
                .HasColumnType("int");

            Property(x => x.ValorTotalBruto)
                .IsRequired()
                .HasColumnType("decimal");

            Property(x => x.ValorTotalFrete)
                .HasColumnType("decimal");

            Property(x => x.ValorTotalLiquido)
                .IsRequired()
                .HasColumnType("decimal");

            //Relacionamento 0 ou 1 : 1
            HasOptional(x => x.Usuario)
                .WithOptionalDependent(x => x.Compra)
                .WillCascadeOnDelete(false);

            //Relacionamento 0 ou 1 : 1
            HasOptional(x => x.Fornecedor)
                .WithOptionalDependent(x => x.Compra)
                .WillCascadeOnDelete(false);

            //Relacionamento 0 ou 1 : 1
            HasOptional(x => x.CondicaoPagamento)
                .WithOptionalDependent(x => x.Compra)
                .WillCascadeOnDelete(false);

            //Relacionamento 0 ou 1 : 1
            HasOptional(x => x.FormaPagamento)
                .WithOptionalDependent(x => x.Compra)
                .WillCascadeOnDelete(false);

            //Relacionamento 1 : N
            HasMany(x => x.ItensCompra)
                .WithRequired(x => x.Compra)
                .WillCascadeOnDelete(true);
        }
    }
}
