namespace ProjetoArtCouro.DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alteracaodeusuario : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Usuario", "UsuarioId", "dbo.PessoaFisica");
            DropIndex("dbo.Usuario", new[] { "UsuarioId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Usuario", "UsuarioId");
            AddForeignKey("dbo.Usuario", "UsuarioId", "dbo.PessoaFisica", "PessoaFisicaId");
        }
    }
}
