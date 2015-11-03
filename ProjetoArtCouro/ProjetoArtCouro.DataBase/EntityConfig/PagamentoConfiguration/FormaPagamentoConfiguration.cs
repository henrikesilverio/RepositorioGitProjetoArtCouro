using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Pagamentos;

namespace ProjetoArtCouro.DataBase.EntityConfig.PagamentoConfiguration
{
    public class FormaPagamentoConfiguration : EntityTypeConfiguration<FormaPagamento>
    {
        public FormaPagamentoConfiguration()
        {
            ToTable("FormaPagamento");

            Property(x => x.FormaPagamentoId)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.FormaPagamentoCodigo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Descricao)
               .IsRequired()
               .HasMaxLength(30)
               .HasColumnType("varchar");

            Property(x => x.Ativo)
                .IsRequired()
                .HasColumnType("bit");
        }
    }
}
