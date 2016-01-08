using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ProjetoArtCouro.DataBase.EntityConfig.CompraConfiguration;
using ProjetoArtCouro.DataBase.EntityConfig.PagamentoConfiguration;
using ProjetoArtCouro.DataBase.EntityConfig.PessoaConfiguration;
using ProjetoArtCouro.DataBase.EntityConfig.ProdutoConfiguration;
using ProjetoArtCouro.DataBase.EntityConfig.UsuarioConfiguration;
using ProjetoArtCouro.DataBase.EntityConfig.VendaConfiguration;
using ProjetoArtCouro.Domain.Models.Compras;
using ProjetoArtCouro.Domain.Models.Pagamentos;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Domain.Models.Produtos;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Domain.Models.Vendas;

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

        public DbSet<CondicaoPagamento> CondicoesPagamento { get; set; }
        public DbSet<ContaReceber> ContasReceber { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<EstadoCivil> EstadosCivis { get; set; }
        public DbSet<FormaPagamento> FormasPagamento { get; set; }
        public DbSet<GrupoPermissao> GruposPermissao { get; set; }
        public DbSet<ItemCompra> ItensCompra { get; set; }
        public DbSet<ItemVenda> ItensVenda { get; set; }
        public DbSet<MeioComunicacao> MeiosComunicacao { get; set; }
        public DbSet<Papel> Papeis { get; set; }
        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<PessoaFisica> PessoasFisicas { get; set; }
        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Unidade> Unidades { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Venda> Vendas { get; set; }

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
            modelBuilder.Configurations.Add(new CondicaoPagamentoConfiguration());
            modelBuilder.Configurations.Add(new ContaReceberConfiguration());
            modelBuilder.Configurations.Add(new CompraConfiguration());
            modelBuilder.Configurations.Add(new EnderecoConfiguration());
            modelBuilder.Configurations.Add(new EstadoConfiguration());
            modelBuilder.Configurations.Add(new EstadoCivilConfiguration());
            modelBuilder.Configurations.Add(new FormaPagamentoConfiguration());
            modelBuilder.Configurations.Add(new GrupoPermissaoConfiguration());
            modelBuilder.Configurations.Add(new ItemCompraConfiguration());
            modelBuilder.Configurations.Add(new ItemVendaConfiguration());
            modelBuilder.Configurations.Add(new MeioComunicacaoConfiguration());
            modelBuilder.Configurations.Add(new PapelConfiguration());
            modelBuilder.Configurations.Add(new PermissaoConfiguration());
            modelBuilder.Configurations.Add(new PessoaConfiguration());
            modelBuilder.Configurations.Add(new PessoaFisicaConfiguration());
            modelBuilder.Configurations.Add(new PessoaJuridicaConfiguration());
            modelBuilder.Configurations.Add(new ProdutoConfiguration());
            modelBuilder.Configurations.Add(new UnidadeConfiguration());
            modelBuilder.Configurations.Add(new UsuarioConfiguration());
            modelBuilder.Configurations.Add(new VendaConfiguration());
        }
    }
}
