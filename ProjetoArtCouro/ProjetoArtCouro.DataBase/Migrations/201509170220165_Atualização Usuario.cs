namespace ProjetoArtCouro.DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtualizaçãoUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "Ativo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuario", "Ativo");
        }
    }
}
