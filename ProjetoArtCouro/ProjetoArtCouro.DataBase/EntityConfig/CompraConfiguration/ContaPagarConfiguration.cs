using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Compras;

namespace ProjetoArtCouro.DataBase.EntityConfig.CompraConfiguration
{
    public class ContaPagarConfiguration : EntityTypeConfiguration<ContaPagar>
    {
        public ContaPagarConfiguration()
        {
            ToTable("ContaPagar");

            Property(x => x.ContaPagarId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.ContaPagarCodigo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.DataVencimento)
                .IsRequired()
                .HasColumnType("datetime2");

            Property(x => x.ValorDocumento)
                .IsRequired()
                .HasColumnType("decimal");

            Property(x => x.StatusContaPagar)
                .IsRequired()
                .HasColumnType("int");

            Property(x => x.Recebido)
                .IsRequired()
                .HasColumnType("bit");
        }
    }
}
