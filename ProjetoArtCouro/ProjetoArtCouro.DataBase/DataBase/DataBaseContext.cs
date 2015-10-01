using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ProjetoArtCouro.DataBase.EntityConfig.PessoaConfiguration;
using ProjetoArtCouro.DataBase.EntityConfig.UsuarioConfiguration;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.DataBase.DataBase
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

        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<GrupoPermissao> GruposPermissao { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<EstadoCivil> EstadosCivis { get; set; }
        public DbSet<MeioComunicacao> MeiosComunicacao { get; set; }
        public DbSet<Papel> Papeis { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<PessoaFisica> PessoasFisicas { get; set; }
        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }
        

        //Customizando criação do DbCOntext
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Removendo pluralização dos nomes das tabelas
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //Removendo delete em cascate dos relacionamentos 1:N
            //modelBuilder.Conventions.Add<OneToManyCascadeDeleteConvention>();
            //Removendo delete em cascate dos relacionamentos N:N
            //modelBuilder.Conventions.Add<ManyToManyCascadeDeleteConvention>();

            //Setando as configurações para criação dos objetos
            modelBuilder.Configurations.Add(new PermissaoConfiguration());
            modelBuilder.Configurations.Add(new GrupoPermissaoConfiguration());
            modelBuilder.Configurations.Add(new UsuarioConfiguration());
            modelBuilder.Configurations.Add(new EnderecoConfiguration());
            modelBuilder.Configurations.Add(new EstadoCivilConfiguration());
            modelBuilder.Configurations.Add(new EstadoConfiguration());
            modelBuilder.Configurations.Add(new MeioComunicacaoConfiguration());
            modelBuilder.Configurations.Add(new PapelConfiguration());
            modelBuilder.Configurations.Add(new PessoaConfiguration());
            modelBuilder.Configurations.Add(new PessoaFisicaConfiguration());
            modelBuilder.Configurations.Add(new PessoaJuridicaConfiguration());
        }
    }
}
