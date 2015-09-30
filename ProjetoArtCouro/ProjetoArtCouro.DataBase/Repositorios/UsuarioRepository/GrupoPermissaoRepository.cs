using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.DataBase.Repositorios.UsuarioRepository
{
    public class GrupoPermissaoRepository : IGrupoPermissaoRepository
    {
        private readonly DataBaseContext _context;
        public GrupoPermissaoRepository(DataBaseContext context)
        {
            _context = context;
        }

        public GrupoPermissao ObterPorId(Guid id)
        {
            return _context.GruposPermissao.FirstOrDefault(x => x.GrupoPermissaoId.Equals(id));
        }

        public GrupoPermissao ObterPorCodigo(int codigo)
        {
            return _context.GruposPermissao.FirstOrDefault(x => x.GrupoPermissaoCodigo.Equals(codigo));
        }

        public List<GrupoPermissao> ObterLista()
        {
            return _context.GruposPermissao.ToList();
        }

        public void Criar(GrupoPermissao gruposPermissao)
        {
            _context.GruposPermissao.Add(gruposPermissao);
            _context.SaveChanges();
        }

        public void Atualizar(GrupoPermissao gruposPermissao)
        {
            _context.Entry(gruposPermissao).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(GrupoPermissao gruposPermissao)
        {
            _context.GruposPermissao.Remove(gruposPermissao);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
