using System.Data.Entity.ModelConfiguration;
using ProjetoArtCouroDomain.Registers;

namespace ProjetoArtCouroDataBase.EntityConfig.Person
{
    public class LegalPersonConfiguration : EntityTypeConfiguration<LegalPerson>
    {
        public LegalPersonConfiguration()
        {
            //Setando a propriedade CandidatoId como primaryKey 
            HasKey(legalPerson => legalPerson.PersonId);

            //Setando que propriedade Nome não pode ser nula e tem 150 como máximo de caracteres 
            //Property(c => c.Nome)
                //.IsRequired()
                //.HasMaxLength(150);

            //Setando que propriedade Email não pode ser nula e tem 150 como máximo de caracteres 
            //Property(c => c.Email)
                //.IsRequired()
                //.HasMaxLength(150);

            //Definindo relacionamento N:N entre Candidatos e Tecnologias
            //HasMany(c => c.Tecnologias).WithMany(c => c.Candidatos)
                               //.Map(t => t.ToTable("Candidatos")
                                   //.MapLeftKey("Candidatos")
                                   //.MapRightKey("Tecnologias"));
        }
    }
}
