namespace ProjetoArtCouro.DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Endereco",
                c => new
                    {
                        EnderecoId = c.Guid(nullable: false, identity: true),
                        CEP = c.String(nullable: false, maxLength: 9, unicode: false),
                        Logradouro = c.String(nullable: false, maxLength: 200, unicode: false),
                        Numero = c.String(nullable: false, maxLength: 6, unicode: false),
                        Bairro = c.String(nullable: false, maxLength: 50, unicode: false),
                        Complemento = c.String(maxLength: 50, unicode: false),
                        Cidade = c.String(nullable: false, maxLength: 50, unicode: false),
                        Principal = c.Boolean(nullable: false),
                        PessoaId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.EnderecoId)
                .ForeignKey("dbo.Estado", t => t.EnderecoId)
                .ForeignKey("dbo.Pessoa", t => t.PessoaId)
                .Index(t => t.EnderecoId)
                .Index(t => t.PessoaId);
            
            CreateTable(
                "dbo.Estado",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Codigo = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 250, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Codigo, unique: true, name: "IX_CODIGO");
            
            CreateTable(
                "dbo.Pessoa",
                c => new
                    {
                        PessoaId = c.Guid(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 150, unicode: false),
                        MeioComunicacaoId = c.Guid(nullable: false),
                        EnderecoId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PessoaId);
            
            CreateTable(
                "dbo.MeioComunicacao",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TipoComunicacao = c.Int(nullable: false),
                        Principal = c.Boolean(nullable: false),
                        PessoaId = c.Guid(nullable: false),
                        Codigo = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 250, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pessoa", t => t.PessoaId)
                .Index(t => t.PessoaId)
                .Index(t => t.Codigo, unique: true, name: "IX_CODIGO");
            
            CreateTable(
                "dbo.Papel",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Codigo = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 250, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Codigo, unique: true, name: "IX_CODIGO");
            
            CreateTable(
                "dbo.PessoaFisica",
                c => new
                    {
                        PessoaFisicaId = c.Guid(nullable: false, identity: true),
                        CPF = c.String(nullable: false, maxLength: 8000, unicode: false),
                        RG = c.String(nullable: false, maxLength: 15, unicode: false),
                        Sexo = c.String(nullable: false, maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.PessoaFisicaId)
                .ForeignKey("dbo.EstadoCivil", t => t.PessoaFisicaId)
                .ForeignKey("dbo.Pessoa", t => t.PessoaFisicaId)
                .Index(t => t.PessoaFisicaId)
                .Index(t => t.CPF, unique: true);
            
            CreateTable(
                "dbo.EstadoCivil",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Codigo = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 250, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Codigo, unique: true, name: "IX_CODIGO");
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        UsuarioId = c.Guid(nullable: false, identity: true),
                        UsuarioNome = c.String(nullable: false, maxLength: 60),
                        Senha = c.String(nullable: false, maxLength: 32, fixedLength: true),
                    })
                .PrimaryKey(t => t.UsuarioId)
                .ForeignKey("dbo.PessoaFisica", t => t.UsuarioId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Permissao",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Codigo = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Codigo, unique: true, name: "IX_CODIGO");
            
            CreateTable(
                "dbo.PessoaJuridica",
                c => new
                    {
                        PessoaJuridicaId = c.Guid(nullable: false, identity: true),
                        CNPJ = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Contato = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.PessoaJuridicaId)
                .ForeignKey("dbo.Pessoa", t => t.PessoaJuridicaId)
                .Index(t => t.PessoaJuridicaId)
                .Index(t => t.CNPJ, unique: true);
            
            CreateTable(
                "dbo.PessoaPapel",
                c => new
                    {
                        PessoaId = c.Guid(nullable: false),
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.PessoaId, t.Id })
                .ForeignKey("dbo.Pessoa", t => t.PessoaId)
                .ForeignKey("dbo.Papel", t => t.Id)
                .Index(t => t.PessoaId)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.UsuarioPermissao",
                c => new
                    {
                        UsuarioId = c.Guid(nullable: false),
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UsuarioId, t.Id })
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .ForeignKey("dbo.Permissao", t => t.Id)
                .Index(t => t.UsuarioId)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Endereco", "PessoaId", "dbo.Pessoa");
            DropForeignKey("dbo.PessoaJuridica", "PessoaJuridicaId", "dbo.Pessoa");
            DropForeignKey("dbo.PessoaFisica", "PessoaFisicaId", "dbo.Pessoa");
            DropForeignKey("dbo.Usuario", "UsuarioId", "dbo.PessoaFisica");
            DropForeignKey("dbo.UsuarioPermissao", "Id", "dbo.Permissao");
            DropForeignKey("dbo.UsuarioPermissao", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.PessoaFisica", "PessoaFisicaId", "dbo.EstadoCivil");
            DropForeignKey("dbo.PessoaPapel", "Id", "dbo.Papel");
            DropForeignKey("dbo.PessoaPapel", "PessoaId", "dbo.Pessoa");
            DropForeignKey("dbo.MeioComunicacao", "PessoaId", "dbo.Pessoa");
            DropForeignKey("dbo.Endereco", "EnderecoId", "dbo.Estado");
            DropIndex("dbo.UsuarioPermissao", new[] { "Id" });
            DropIndex("dbo.UsuarioPermissao", new[] { "UsuarioId" });
            DropIndex("dbo.PessoaPapel", new[] { "Id" });
            DropIndex("dbo.PessoaPapel", new[] { "PessoaId" });
            DropIndex("dbo.PessoaJuridica", new[] { "CNPJ" });
            DropIndex("dbo.PessoaJuridica", new[] { "PessoaJuridicaId" });
            DropIndex("dbo.Permissao", "IX_CODIGO");
            DropIndex("dbo.Usuario", new[] { "UsuarioId" });
            DropIndex("dbo.EstadoCivil", "IX_CODIGO");
            DropIndex("dbo.PessoaFisica", new[] { "CPF" });
            DropIndex("dbo.PessoaFisica", new[] { "PessoaFisicaId" });
            DropIndex("dbo.Papel", "IX_CODIGO");
            DropIndex("dbo.MeioComunicacao", "IX_CODIGO");
            DropIndex("dbo.MeioComunicacao", new[] { "PessoaId" });
            DropIndex("dbo.Estado", "IX_CODIGO");
            DropIndex("dbo.Endereco", new[] { "PessoaId" });
            DropIndex("dbo.Endereco", new[] { "EnderecoId" });
            DropTable("dbo.UsuarioPermissao");
            DropTable("dbo.PessoaPapel");
            DropTable("dbo.PessoaJuridica");
            DropTable("dbo.Permissao");
            DropTable("dbo.Usuario");
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
