namespace ProjetoArtCouro.DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InicioBanco : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Endereco",
                c => new
                    {
                        EnderecoId = c.Guid(nullable: false, identity: true),
                        EnderecoCodigo = c.Int(nullable: false, identity: true),
                        CEP = c.String(nullable: false, maxLength: 9, unicode: false),
                        Logradouro = c.String(nullable: false, maxLength: 200, unicode: false),
                        Numero = c.String(nullable: false, maxLength: 6, unicode: false),
                        Bairro = c.String(nullable: false, maxLength: 50, unicode: false),
                        Complemento = c.String(maxLength: 50, unicode: false),
                        Cidade = c.String(nullable: false, maxLength: 50, unicode: false),
                        Principal = c.Boolean(nullable: false),
                        PessoaId = c.Guid(nullable: false),
                        Estado_EstadoId = c.Guid(),
                    })
                .PrimaryKey(t => t.EnderecoId)
                .ForeignKey("dbo.Estado", t => t.Estado_EstadoId)
                .ForeignKey("dbo.Pessoa", t => t.PessoaId, cascadeDelete: true)
                .Index(t => t.PessoaId)
                .Index(t => t.Estado_EstadoId);
            
            CreateTable(
                "dbo.Estado",
                c => new
                    {
                        EstadoId = c.Guid(nullable: false, identity: true),
                        EstadoCodigo = c.Int(nullable: false, identity: true),
                        EstadoNome = c.String(nullable: false, maxLength: 250, unicode: false),
                    })
                .PrimaryKey(t => t.EstadoId);
            
            CreateTable(
                "dbo.Pessoa",
                c => new
                    {
                        PessoaId = c.Guid(nullable: false, identity: true),
                        PessoaCodigo = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 150, unicode: false),
                    })
                .PrimaryKey(t => t.PessoaId);
            
            CreateTable(
                "dbo.MeioComunicacao",
                c => new
                    {
                        MeioComunicacaoId = c.Guid(nullable: false, identity: true),
                        MeioComunicacaoCodigo = c.Int(nullable: false, identity: true),
                        MeioComunicacaoNome = c.String(nullable: false, maxLength: 250, unicode: false),
                        TipoComunicacao = c.Int(nullable: false),
                        Principal = c.Boolean(nullable: false),
                        PessoaId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.MeioComunicacaoId)
                .ForeignKey("dbo.Pessoa", t => t.PessoaId, cascadeDelete: true)
                .Index(t => t.PessoaId);
            
            CreateTable(
                "dbo.Papel",
                c => new
                    {
                        PapelId = c.Guid(nullable: false, identity: true),
                        PapelCodigo = c.Int(nullable: false, identity: true),
                        PapelNome = c.String(nullable: false, maxLength: 250, unicode: false),
                    })
                .PrimaryKey(t => t.PapelId);
            
            CreateTable(
                "dbo.PessoaFisica",
                c => new
                    {
                        PessoaId = c.Guid(nullable: false),
                        PessoaFisicaCodigo = c.Int(nullable: false, identity: true),
                        CPF = c.String(nullable: false, maxLength: 8000, unicode: false),
                        RG = c.String(nullable: false, maxLength: 15, unicode: false),
                        Sexo = c.String(nullable: false, maxLength: 10, unicode: false),
                        EstadoCivil_EstadoCivilId = c.Guid(),
                    })
                .PrimaryKey(t => t.PessoaId)
                .ForeignKey("dbo.EstadoCivil", t => t.EstadoCivil_EstadoCivilId)
                .ForeignKey("dbo.Pessoa", t => t.PessoaId)
                .Index(t => t.PessoaId)
                .Index(t => t.CPF, unique: true)
                .Index(t => t.EstadoCivil_EstadoCivilId);
            
            CreateTable(
                "dbo.EstadoCivil",
                c => new
                    {
                        EstadoCivilId = c.Guid(nullable: false, identity: true),
                        EstadoCivilCodigo = c.Int(nullable: false, identity: true),
                        EstadoCivilNome = c.String(nullable: false, maxLength: 250, unicode: false),
                    })
                .PrimaryKey(t => t.EstadoCivilId);
            
            CreateTable(
                "dbo.PessoaJuridica",
                c => new
                    {
                        PessoaId = c.Guid(nullable: false),
                        PessoaJuridicaCodigo = c.Int(nullable: false, identity: true),
                        CNPJ = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Contato = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.PessoaId)
                .ForeignKey("dbo.Pessoa", t => t.PessoaId)
                .Index(t => t.PessoaId)
                .Index(t => t.CNPJ, unique: true);
            
            CreateTable(
                "dbo.Permissao",
                c => new
                    {
                        PermissaoId = c.Guid(nullable: false, identity: true),
                        PermissaoCodigo = c.Int(nullable: false, identity: true),
                        PermissaoNome = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.PermissaoId);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        UsuarioId = c.Guid(nullable: false, identity: true),
                        UsuarioCodigo = c.Int(nullable: false, identity: true),
                        UsuarioNome = c.String(nullable: false, maxLength: 60),
                        Senha = c.String(nullable: false, maxLength: 32, fixedLength: true),
                    })
                .PrimaryKey(t => t.UsuarioId)
                .Index(t => t.UsuarioCodigo, unique: true, name: "IX_USUARIO_CODIGO");
            
            CreateTable(
                "dbo.PessoaPapel",
                c => new
                    {
                        PessoaId = c.Guid(nullable: false),
                        PapelId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.PessoaId, t.PapelId })
                .ForeignKey("dbo.Pessoa", t => t.PessoaId)
                .ForeignKey("dbo.Papel", t => t.PapelId)
                .Index(t => t.PessoaId)
                .Index(t => t.PapelId);
            
            CreateTable(
                "dbo.UsuarioPermissao",
                c => new
                    {
                        UsuarioId = c.Guid(nullable: false),
                        PermissaoId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UsuarioId, t.PermissaoId })
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .ForeignKey("dbo.Permissao", t => t.PermissaoId)
                .Index(t => t.UsuarioId)
                .Index(t => t.PermissaoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsuarioPermissao", "PermissaoId", "dbo.Permissao");
            DropForeignKey("dbo.UsuarioPermissao", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.PessoaJuridica", "PessoaId", "dbo.Pessoa");
            DropForeignKey("dbo.PessoaFisica", "PessoaId", "dbo.Pessoa");
            DropForeignKey("dbo.PessoaFisica", "EstadoCivil_EstadoCivilId", "dbo.EstadoCivil");
            DropForeignKey("dbo.PessoaPapel", "PapelId", "dbo.Papel");
            DropForeignKey("dbo.PessoaPapel", "PessoaId", "dbo.Pessoa");
            DropForeignKey("dbo.MeioComunicacao", "PessoaId", "dbo.Pessoa");
            DropForeignKey("dbo.Endereco", "PessoaId", "dbo.Pessoa");
            DropForeignKey("dbo.Endereco", "Estado_EstadoId", "dbo.Estado");
            DropIndex("dbo.UsuarioPermissao", new[] { "PermissaoId" });
            DropIndex("dbo.UsuarioPermissao", new[] { "UsuarioId" });
            DropIndex("dbo.PessoaPapel", new[] { "PapelId" });
            DropIndex("dbo.PessoaPapel", new[] { "PessoaId" });
            DropIndex("dbo.Usuario", "IX_USUARIO_CODIGO");
            DropIndex("dbo.PessoaJuridica", new[] { "CNPJ" });
            DropIndex("dbo.PessoaJuridica", new[] { "PessoaId" });
            DropIndex("dbo.PessoaFisica", new[] { "EstadoCivil_EstadoCivilId" });
            DropIndex("dbo.PessoaFisica", new[] { "CPF" });
            DropIndex("dbo.PessoaFisica", new[] { "PessoaId" });
            DropIndex("dbo.MeioComunicacao", new[] { "PessoaId" });
            DropIndex("dbo.Endereco", new[] { "Estado_EstadoId" });
            DropIndex("dbo.Endereco", new[] { "PessoaId" });
            DropTable("dbo.UsuarioPermissao");
            DropTable("dbo.PessoaPapel");
            DropTable("dbo.Usuario");
            DropTable("dbo.Permissao");
            DropTable("dbo.PessoaJuridica");
            DropTable("dbo.EstadoCivil");
            DropTable("dbo.PessoaFisica");
            DropTable("dbo.Papel");
            DropTable("dbo.MeioComunicacao");
            DropTable("dbo.Pessoa");
            DropTable("dbo.Estado");
            DropTable("dbo.Endereco");
        }
    }
}