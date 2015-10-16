using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.Domain.Contracts.IService.IUsuario
{
    public interface IUsuarioService : IDisposable
    {
        void Registrar(string nome, string senha, string confirmaSenha, int codigoGrupo);
        void AlterarSenha(Usuario usuario);
        void EditarUsuario(Usuario usuario);
        void ExcluirUsuario(int codigoUsuario);
        List<Usuario> ObterListaUsuario();
        List<Permissao> ObterListaPermissao();
        List<Usuario> PesquisarUsuario(string nome, int? permissaoId, bool? ativo);
        Usuario PesquisarUsuarioPorCodigo(int codigoUsuario);
        List<Permissao> ObterPermissoesUsuarioLogado(string usuarioNome);
        GrupoPermissao ObterGrupoPermissaoPorCodigo(int codigo);
        List<GrupoPermissao> PesquisarGrupo(string nome, int? codigo, bool todos);
        List<GrupoPermissao> ObterListaGrupoPermissao();
        void CriarGrupoPermissao(GrupoPermissao grupoPermissao);
        void EditarGrupoPermissao(GrupoPermissao grupoPermissao);
        void EditarPermissaoUsuario(int codigoUsuario, List<Permissao> permissoes);
        void ExcluirGrupoPermissao(int codigoGrupoPermissao);
    }
}
