namespace ProjetoArtCouro.DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inclusaodocodigocomincremetal : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Estado", "IX_CODIGO");
            DropIndex("dbo.MeioComunicacao", "IX_CODIGO");
            DropIndex("dbo.Papel", "IX_CODIGO");
            DropIndex("dbo.EstadoCivil", "IX_CODIGO");
            DropIndex("dbo.Permissao", "IX_CODIGO");
            AddColumn("dbo.Endereco", "EnderecoCodigo", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Pessoa", "PessoaCodigo", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Usuario", "UsuarioCodigo", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Estado", "Codigo", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.MeioComunicacao", "Codigo", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Papel", "Codigo", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.EstadoCivil", "Codigo", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Permissao", "Codigo", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.Endereco", "EnderecoCodigo", unique: true, name: "IX_ENDERECO_CODIGO");
            CreateIndex("dbo.Estado", "Codigo", unique: true, name: "IX_CODIGO");
            CreateIndex("dbo.Pessoa", "PessoaCodigo", unique: true, name: "IX_PESSOA_CODIGO");
            CreateIndex("dbo.MeioComunicacao", "Codigo", unique: true, name: "IX_CODIGO");
            CreateIndex("dbo.Papel", "Codigo", unique: true, name: "IX_CODIGO");
            CreateIndex("dbo.EstadoCivil", "Codigo", unique: true, name: "IX_CODIGO");
            CreateIndex("dbo.Permissao", "Codigo", unique: true, name: "IX_CODIGO");
            CreateIndex("dbo.Usuario", "UsuarioCodigo", unique: true, name: "IX_USUARIO_CODIGO");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Usuario", "IX_USUARIO_CODIGO");
            DropIndex("dbo.Permissao", "IX_CODIGO");
            DropIndex("dbo.EstadoCivil", "IX_CODIGO");
            DropIndex("dbo.Papel", "IX_CODIGO");
            DropIndex("dbo.MeioComunicacao", "IX_CODIGO");
            DropIndex("dbo.Pessoa", "IX_PESSOA_CODIGO");
            DropIndex("dbo.Estado", "IX_CODIGO");
            DropIndex("dbo.Endereco", "IX_ENDERECO_CODIGO");
            AlterColumn("dbo.Permissao", "Codigo", c => c.Int(nullable: false));
            AlterColumn("dbo.EstadoCivil", "Codigo", c => c.Int(nullable: false));
            AlterColumn("dbo.Papel", "Codigo", c => c.Int(nullable: false));
            AlterColumn("dbo.MeioComunicacao", "Codigo", c => c.Int(nullable: false));
            AlterColumn("dbo.Estado", "Codigo", c => c.Int(nullable: false));
            DropColumn("dbo.Usuario", "UsuarioCodigo");
            DropColumn("dbo.Pessoa", "PessoaCodigo");
            DropColumn("dbo.Endereco", "EnderecoCodigo");
            CreateIndex("dbo.Permissao", "Codigo", unique: true, name: "IX_CODIGO");
            CreateIndex("dbo.EstadoCivil", "Codigo", unique: true, name: "IX_CODIGO");
            CreateIndex("dbo.Papel", "Codigo", unique: true, name: "IX_CODIGO");
            CreateIndex("dbo.MeioComunicacao", "Codigo", unique: true, name: "IX_CODIGO");
            CreateIndex("dbo.Estado", "Codigo", unique: true, name: "IX_CODIGO");
        }
    }
}
