using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario;
using ProjetoArtCouro.Domain.Contracts.IService.IUsuario;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Business.Services.UsuarioService
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPermissaoRepository _permissaoRepository;
        private readonly IGrupoPermissaoRepository _grupoPermissaoRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository, IPermissaoRepository permissaoRepository,
            IGrupoPermissaoRepository grupoPermissaoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _permissaoRepository = permissaoRepository;
            _grupoPermissaoRepository = grupoPermissaoRepository;
        }

        public void Registrar(string nome, string senha, string confimaSenha, int codigoGrupo)
        {
            AssertionConcern.AssertArgumentNotEmpty(nome, Erros.InvalidUserName);
            var temUsuario = _usuarioRepository.ObterPorUsuarioNome(nome.ToLower());
            if (temUsuario != null)
            {
                throw new Exception(Erros.DuplicateUserName);
            };
            var temGrupo = _grupoPermissaoRepository.ObterPorCodigoComPermissoes(codigoGrupo);
            AssertionConcern.AssertArgumentNotEquals(temGrupo, null, Erros.GroupDoesNotExist);
            var listaPermissao = temGrupo.Permissoes;
            if (!listaPermissao.Any())
            {
                throw new Exception(Erros.PermissionsNotRegistered);
            }

            var usuario = new Usuario()
            {
                UsuarioNome = nome.ToLower(),
                Senha = PasswordAssertionConcern.Encrypt(senha),
                Ativo = true,
                Permissoes = listaPermissao,
                GrupoPermissao = temGrupo
            };

            usuario.Validar();
            _usuarioRepository.Criar(usuario);
        }

        public void AlterarSenha(Usuario usuario)
        {
            AssertionConcern.AssertArgumentNotEmpty(usuario.UsuarioNome, Erros.InvalidUserName);
            var usuarioAtual = _usuarioRepository.ObterComPermissoesPorUsuarioNome(usuario.UsuarioNome);
            usuarioAtual.Senha = PasswordAssertionConcern.Encrypt(usuario.Senha);
            usuarioAtual.Validar();
            _usuarioRepository.Atualizar(usuarioAtual);
        }

        public void EditarUsuario(Usuario usuario)
        {
            var usuarioAtual = _usuarioRepository.ObterPorCodigoComPermissoesEGrupo(usuario.UsuarioCodigo);
            AssertionConcern.AssertArgumentNotNull(usuarioAtual, Erros.UserDoesNotExist);
            var grupoAtual = usuarioAtual.GrupoPermissao;
            var novoGrupo =
                _grupoPermissaoRepository.ObterPorCodigoComPermissoes(usuario.GrupoPermissao.GrupoPermissaoCodigo);
            AssertionConcern.AssertArgumentNotNull(novoGrupo, Erros.GroupDoesNotExist);
            var permissoesDoUsuario = usuarioAtual.Permissoes.ToList();
            //Remove todas as permissões que pertencem ao grupo de permissões atual
            permissoesDoUsuario.RemoveAll(x => grupoAtual.Permissoes.Any(a => a.PermissaoId.Equals(x.PermissaoId)));
            //Adiciona as permissões do novo grupo de permissões
            novoGrupo.Permissoes.ToList().ForEach(x =>
            {
                permissoesDoUsuario.Add(x);
            });
            usuarioAtual.Ativo = usuario.Ativo;
            usuarioAtual.UsuarioNome = usuario.UsuarioNome;
            usuarioAtual.Permissoes.Clear();
            usuarioAtual.Permissoes = permissoesDoUsuario;
            usuarioAtual.GrupoPermissao = null;
            usuarioAtual.GrupoPermissao = novoGrupo;
            if (!string.IsNullOrEmpty(usuario.Senha))
            {
                usuarioAtual.Senha = PasswordAssertionConcern.Encrypt(usuario.Senha);
            }
            _usuarioRepository.Atualizar(usuarioAtual);
        }

        public void ExcluirUsuario(int codigoUsuario)
        {
            var usuario = _usuarioRepository.ObterPorCodigo(codigoUsuario);
            AssertionConcern.AssertArgumentNotNull(usuario, Erros.UserDoesNotExist);
            _usuarioRepository.Deletar(usuario);
        }

        public List<Usuario> ObterListaUsuario()
        {
            return _usuarioRepository.ObterListaComPermissoes();
        }

        public List<Permissao> ObterListaPermissao()
        {
            return _permissaoRepository.ObterLista();
        }

        public List<Usuario> PesquisarUsuario(string nome, int? codigoGrupo, bool? ativo)
        {
            return _usuarioRepository.ObterLista(nome, codigoGrupo, ativo);
        }

        public Usuario PesquisarUsuarioPorCodigo(int codigoUsuario)
        {
            return _usuarioRepository.ObterPorCodigoComPermissoesEGrupo(codigoUsuario);
        }

        public List<Permissao> ObterPermissoesUsuarioLogado(string usuarioNome)
        {
            var usuario = _usuarioRepository.ObterComPermissoesPorUsuarioNome(usuarioNome);
            return usuario.Permissoes.ToList();
        }

        public GrupoPermissao ObterGrupoPermissaoPorCodigo(int codigo)
        {
            return _grupoPermissaoRepository.ObterPorCodigoComPermissoes(codigo);
        }

        public List<GrupoPermissao> PesquisarGrupo(string nome, int? codigo, bool todos)
        {
            return todos ? _grupoPermissaoRepository.ObterLista() : _grupoPermissaoRepository.ObterLista(nome, codigo);
        }

        public List<GrupoPermissao> ObterListaGrupoPermissao()
        {
            return _grupoPermissaoRepository.ObterLista();
        }

        public void CriarGrupoPermissao(GrupoPermissao grupoPermissao)
        {
            AssertionConcern.AssertArgumentNotEmpty(grupoPermissao.GrupoPermissaoNome, Erros.EmptyGroupName);
            var temGrupo = _grupoPermissaoRepository.ObterPorGrupoPermissaoNome(grupoPermissao.GrupoPermissaoNome.ToLower());
            if (temGrupo != null)
            {
                throw new Exception(Erros.DuplicateGruopName);
            };
            AtualizarListaPermissao(grupoPermissao);
            _grupoPermissaoRepository.Criar(grupoPermissao);
        }

        public void EditarGrupoPermissao(GrupoPermissao grupoPermissao)
        {
            AssertionConcern.AssertArgumentNotEmpty(grupoPermissao.GrupoPermissaoNome, Erros.EmptyGroupName);
            var bdGrupoPermissao =
                _grupoPermissaoRepository.ObterPorCodigoComPermissoesEUsuarios(grupoPermissao.GrupoPermissaoCodigo);
            var listaPermissao = _permissaoRepository.ObterLista();
            if (!listaPermissao.Any())
            {
                throw new Exception(Erros.PermissionsNotRegistered);
            }
            bdGrupoPermissao.Permissoes.Clear();
            var listaAdicionar = grupoPermissao.Permissoes.Select(x =>
                listaPermissao.FirstOrDefault(a => a.PermissaoCodigo.Equals(x.PermissaoCodigo))).ToList();
            listaAdicionar.ForEach(x =>
            {
                bdGrupoPermissao.Permissoes.Add(x);
            });
            _grupoPermissaoRepository.Atualizar(bdGrupoPermissao);
            AtualizarPermissoesDeUsuariosPorGrupoPermissao(bdGrupoPermissao);
        }

        public void EditarPermissaoUsuario(int codigoUsuario, List<Permissao> permissoes)
        {
            AssertionConcern.AssertArgumentNotEquals(codigoUsuario, 0, Erros.UserDoesNotExist);
            if (!permissoes.Any())
            {
                throw new Exception(Erros.EmptyAllowList);
            }
            var temUsuario = _usuarioRepository.ObterPorCodigoComPermissoes(codigoUsuario);
            AssertionConcern.AssertArgumentNotEquals(temUsuario, null, Erros.UserDoesNotExist);
            var listaPermissao = _permissaoRepository.ObterLista();
            permissoes = permissoes.Select(x =>
                listaPermissao.FirstOrDefault(a => a.PermissaoCodigo.Equals(x.PermissaoCodigo))).ToList();
            temUsuario.Permissoes.Clear();
            temUsuario.Permissoes = permissoes;
            _usuarioRepository.Atualizar(temUsuario);
        }

        public void ExcluirGrupoPermissao(int codigoGrupoPermissao)
        {
            var grupoPermissao = _grupoPermissaoRepository.ObterPorCodigo(codigoGrupoPermissao);
            AssertionConcern.AssertArgumentNotNull(grupoPermissao, Erros.GroupDoesNotExist);
            _grupoPermissaoRepository.Deletar(grupoPermissao);
        }

        private void AtualizarListaPermissao(GrupoPermissao grupoPermissao)
        {
            var listaPermissao = _permissaoRepository.ObterLista();
            if (!listaPermissao.Any())
            {
                throw new Exception(Erros.PermissionsNotRegistered);
            }
            grupoPermissao.Permissoes = grupoPermissao.Permissoes.Select(x =>
                listaPermissao.FirstOrDefault(a => a.PermissaoCodigo.Equals(x.PermissaoCodigo))).ToList();
            grupoPermissao.GrupoPermissaoNome = grupoPermissao.GrupoPermissaoNome.ToUpper();
        }

        private void AtualizarPermissoesDeUsuariosPorGrupoPermissao(GrupoPermissao grupoPermissao)
        {
            var usuarios =
                grupoPermissao.Usuarios.Select(
                    x => _usuarioRepository.ObterPorCodigoComPermissoesEGrupo(x.UsuarioCodigo));
            foreach (var usuario in usuarios)
            {
                usuario.Permissoes.Clear();
                usuario.Permissoes = grupoPermissao.Permissoes;
                _usuarioRepository.Atualizar(usuario);
            }
        }

        public void Dispose()
        {
            _usuarioRepository.Dispose();
        }
    }
}
