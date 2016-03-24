using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Estoques;

namespace ProjetoArtCouro.DataBase.EntityConfig.EstoqueConfiguration
{
    public class EstoqueConfiguration : EntityTypeConfiguration<Estoque>
    {
        public EstoqueConfiguration()
        {
            ToTable("Estoque");

            Property(x => x.EstoqueId)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.EstoqueCodigo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.DataUltimaEntrada)
               .HasColumnType("datetime2");

            Property(x => x.Quantidade)
                .HasColumnType("int");

            //Relacionamento 0 ou 1 : 1
            HasOptional(x => x.Produto)
                .WithOptionalDependent(x => x.Estoque)
                .WillCascadeOnDelete(false);

            //Relacionamento 0 ou 1 : N
            HasOptional(x => x.Compra)
                .WithMany(x => x.Estoques);
        }
    }
}
