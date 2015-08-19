﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.DataBase.EntityConfig.PessoaConfiguration
{
    public class EnderecoConfiguration : EntityTypeConfiguration<Endereco>
    {
        public EnderecoConfiguration()
        {
            ToTable("Endereco");

            Property(x => x.EnderecoId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.EnderecoCodigo)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_ENDERECO_CODIGO", 1) { IsUnique = true }));

            Property(x => x.CEP)
                .IsRequired()
                .HasMaxLength(9)
                .HasColumnType("varchar");

            Property(x => x.Logradouro)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("varchar");

            Property(x => x.Bairro)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar");

            Property(x => x.Numero)
                .IsRequired()
                .HasMaxLength(6)
                .HasColumnType("varchar");

            Property(x => x.Complemento)
                .HasMaxLength(50)
                .HasColumnType("varchar");

            Property(x => x.Cidade)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar");

            //Relacionamento 1 pra 1 obrigatorio
            HasRequired(x => x.Estado)
                .WithOptional(x => x.Endereco);

            //Relacionamento 1 pra N obrigatorio
            HasRequired(x => x.Pessoa)
                .WithMany(x => x.Enderecos)
                .HasForeignKey(x => x.PessoaId);
        }
    }
}
