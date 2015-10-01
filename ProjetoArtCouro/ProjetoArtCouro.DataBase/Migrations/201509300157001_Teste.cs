namespace ProjetoArtCouro.DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Teste : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PessoaPapel", "PessoaId", "dbo.Pessoa");
            DropForeignKey("dbo.PessoaPapel", "PapelId", "dbo.Papel");
            DropForeignKey("dbo.PermissaoGrupoPermissao", "PermissaoId", "dbo.Permissao");
            DropForeignKey("dbo.PermissaoGrupoPermissao", "GrupoPermissaoId", "dbo.GrupoPermissao");
            DropForeignKey("dbo.UsuarioPermissao", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioPermissao", "PermissaoId", "dbo.Permissao");
            AddForeignKey("dbo.PessoaPapel", "PessoaId", "dbo.Pessoa", "PessoaId", cascadeDelete: true);
            AddForeignKey("dbo.PessoaPapel", "PapelId", "dbo.Papel", "PapelId", cascadeDelete: true);
            AddForeignKey("dbo.PermissaoGrupoPermissao", "PermissaoId", "dbo.Permissao", "PermissaoId", cascadeDelete: true);
            AddForeignKey("dbo.PermissaoGrupoPermissao", "GrupoPermissaoId", "dbo.GrupoPermissao", "GrupoPermissaoId", cascadeDelete: true);
            AddForeignKey("dbo.UsuarioPermissao", "UsuarioId", "dbo.Usuario", "UsuarioId", cascadeDelete: true);
            AddForeignKey("dbo.UsuarioPermissao", "PermissaoId", "dbo.Permissao", "PermissaoId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsuarioPermissao", "PermissaoId", "dbo.Permissao");
            DropForeignKey("dbo.UsuarioPermissao", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.PermissaoGrupoPermissao", "GrupoPermissaoId", "dbo.GrupoPermissao");
            DropForeignKey("dbo.PermissaoGrupoPermissao", "PermissaoId", "dbo.Permissao");
            DropForeignKey("dbo.PessoaPapel", "PapelId", "dbo.Papel");
            DropForeignKey("dbo.PessoaPapel", "PessoaId", "dbo.Pessoa");
            AddForeignKey("dbo.UsuarioPermissao", "PermissaoId", "dbo.Permissao", "PermissaoId");
            AddForeignKey("dbo.UsuarioPermissao", "UsuarioId", "dbo.Usuario", "UsuarioId");
            AddForeignKey("dbo.PermissaoGrupoPermissao", "GrupoPermissaoId", "dbo.GrupoPermissao", "GrupoPermissaoId");
            AddForeignKey("dbo.PermissaoGrupoPermissao", "PermissaoId", "dbo.Permissao", "PermissaoId");
            AddForeignKey("dbo.PessoaPapel", "PapelId", "dbo.Papel", "PapelId");
            AddForeignKey("dbo.PessoaPapel", "PessoaId", "dbo.Pessoa", "PessoaId");
        }
    }
}
