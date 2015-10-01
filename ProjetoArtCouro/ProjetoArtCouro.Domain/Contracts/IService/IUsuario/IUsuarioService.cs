using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.Domain.Contracts.IService.IUsuario
{
    public interface IUsuarioService : IDisposable
    {
        void Registrar(string nome, string senha, string confirmaSenha, List<Permissao> permissoes);
        void AlterarSenha(Usuario usuario, string senha, string novaSenha, string confirmaNovaSenha);
        List<Usuario> ObterLista();
        List<Permissao> ObterListaPermissao();
        List<Usuario> PesquisarUsuario(string nome, int? permissaoId, bool? ativo);
        List<GrupoPermissao> PesquisarGrupo(string nome, int? codigo);
        void CriarGrupoPermissao(GrupoPermissao grupoPermissao);
        void ExcluirGrupoPermissao(int codigoGrupoPermissao);
    }
}
