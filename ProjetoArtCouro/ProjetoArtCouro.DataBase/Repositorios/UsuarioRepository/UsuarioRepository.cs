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
