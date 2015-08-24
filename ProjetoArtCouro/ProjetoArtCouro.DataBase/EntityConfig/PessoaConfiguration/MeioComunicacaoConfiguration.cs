using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.DataBase.EntityConfig.PessoaConfiguration
{
    public class MeioComunicacaoConfiguration : EntityTypeConfiguration<MeioComunicacao>
    {
        public MeioComunicacaoConfiguration()
        {
            ToTable("MeioComunicacao");

            Property(x => x.MeioComunicacaoId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.MeioComunicacaoCodigo)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.MeioComunicacaoNome)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnType("varchar");

            Property(x => x.TipoComunicacao)
                .IsRequired()
                .HasColumnType("int");

            Property(x => x.Principal)
                .IsRequired()
                .HasColumnType("bit");
        }
    }
}
