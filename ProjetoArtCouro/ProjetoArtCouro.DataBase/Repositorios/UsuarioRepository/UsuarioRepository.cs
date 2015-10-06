using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.DataBase.Repositorios.UsuarioRepository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DataBaseContext _context;
        public UsuarioRepository(DataBaseContext context)
        {
            _context = context;
        }

        public Usuario ObterPorId(Guid id)
        {
            return _context.Usuarios.FirstOrDefault(x => x.UsuarioId.Equals(id));
        }

        public Usuario ObterPorCodigo(int codigo)
        {
            return _context.Usuarios.FirstOrDefault(x => x.UsuarioCodigo.Equals(codigo));
        }

        public Usuario ObterPorCodigoComPermissoes(int codigo)
        {
            return _context.Usuarios.Include("Permissoes").FirstOrDefault(x => x.UsuarioCodigo.Equals(codigo));
        }

        public Usuario ObterPorCodigoComPermissoesEGrupo(int codigo)
        {
            return
                _context.Usuarios
                    .Include("Permissoes")
                    .Include("GrupoPermissao")
                    .FirstOrDefault(x => x.UsuarioCodigo.Equals(codigo));
        }

        public Usuario ObterPorUsuarioNome(string usuarioNome)
        {
            return _context.Usuarios.FirstOrDefault(x => x.UsuarioNome.Equals(usuarioNome));
        }

        public Usuario ObterComPermissoesPorUsuarioNome(string usuarioNome)
        {
            return _context.Usuarios.Include("Permissoes").FirstOrDefault(x => x.UsuarioNome.Equals(usuarioNome));
        }

        public List<Usuario> ObterLista()
        {
            return _context.Usuarios.ToList();
        }

        public List<Usuario> ObterListaComPermissoes()
        {
            return _context.Usuarios.Include("Permissoes").ToList();
        }

        public List<Usuario> ObterLista(string nome, int? codigoGrupo, bool? ativo)
        {
            var query = from usuario in _context.Usuarios
                .Include("GrupoPermissao")
                        select usuario;

            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(x => x.UsuarioNome == nome);
            }

            if (codigoGrupo != null && !codigoGrupo.Equals(0))
            {
                query = query.Where(x => x.GrupoPermissao.GrupoPermissaoCodigo == codigoGrupo);
            }

            if (ativo != null)
            {
                query = query.Where(x => x.Ativo == ativo);
            }

            return query.ToList();
        }

        public void Criar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public void Atualizar(Usuario usuario)
        {
            _context.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
