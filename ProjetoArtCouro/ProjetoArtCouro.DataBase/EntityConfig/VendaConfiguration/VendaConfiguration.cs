using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Vendas;

namespace ProjetoArtCouro.DataBase.EntityConfig.VendaConfiguration
{
    public class VendaConfiguration : EntityTypeConfiguration<Venda>
    {
        public VendaConfiguration()
        {
            ToTable("Venda");

            Property(x => x.VendaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.VendaCodigo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.DataCadastro)
                .IsRequired()
                .HasColumnType("datetime2");

            Property(x => x.StatusVenda)
                .IsRequired()
                .HasColumnType("int");

            Property(x => x.ValorTotalBruto)
                .IsRequired()
                .HasColumnType("decimal");

            Property(x => x.ValorTotalDesconto)
                .HasColumnType("decimal");

            Property(x => x.ValorTotalLiquido)
                .IsRequired()
                .HasColumnType("decimal");

            //Relacionamento 0 ou 1 : 1
            HasOptional(x => x.Usuario)
                .WithOptionalDependent(x => x.Venda)
                .WillCascadeOnDelete(false);

            //Relacionamento 0 ou 1 : 1
            HasOptional(x => x.Cliente)
                .WithOptionalDependent(x => x.Venda)
                .WillCascadeOnDelete(false);

            //Relacionamento 0 ou 1 : 1
            HasOptional(x => x.CondicaoPagamento)
                .WithOptionalDependent(x => x.Venda)
                .WillCascadeOnDelete(false);

            //Relacionamento 0 ou 1 : 1
            HasOptional(x => x.FormaPagamento)
                .WithOptionalDependent(x => x.Venda)
                .WillCascadeOnDelete(false);

            //Relacionamento 1 : N
            HasMany(x => x.ItensVenda)
                .WithRequired(x => x.Venda)
                .WillCascadeOnDelete(true);
        }
    }
}
