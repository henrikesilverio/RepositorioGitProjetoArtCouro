using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ProjetoArtCouroDataBase.EntityConfig.Common;
using ProjetoArtCouroDataBase.EntityConfig.Person;
using ProjetoArtCouroDomain.Registers;
using ProjetoArtCouroDomain.GeneralUse;

namespace ProjetoArtCouroDataBase.DataBase
{
    public class DataBaseContext : DbContext
    {
        //Definindo construtor e passando nome da connectionString para acesso a banco de dados
        public DataBaseContext()
            : base("ProjetoArtCouroConnectionString")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<PhysicalPerson> PhysicalPerson { get; set; }
        public DbSet<LegalPerson> LegalPerson { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<MaritalStatus> MaritalStatus { get; set; }

        //Customizando criação do DbCOntext
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Removendo pluralização dos nomes das tabelas
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //Removendo delete em cascate dos relacionamentos 1:N
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //Removendo delete em cascate dos relacionamentos N:N
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //Setando as configurações para criação dos objetos
            modelBuilder.Configurations.Add(new PhysicalPersonConfiguration());
            modelBuilder.Configurations.Add(new LegalPersonConfiguration());
            modelBuilder.Configurations.Add(new UsersConfiguration());
            modelBuilder.Configurations.Add(new StatesConfiguration());
            modelBuilder.Configurations.Add(new MaritalStatusConfiguration());
        }
    }
}
