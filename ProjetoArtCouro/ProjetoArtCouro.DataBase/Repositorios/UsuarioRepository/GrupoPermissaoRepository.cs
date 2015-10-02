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

        public GrupoPermissao ObterPorCodigoComPermissao(int codigo)
        {
            return _context.GruposPermissao.Include("Permissoes").FirstOrDefault(x => x.GrupoPermissaoCodigo.Equals(codigo));
        }

        public GrupoPermissao ObterPorGrupoPermissaoNome(string nome)
        {
            return _context.GruposPermissao.FirstOrDefault(x => x.GrupoPermissaoNome.Equals(nome));
        }

        public List<GrupoPermissao> ObterLista()
        {
            return _context.GruposPermissao.ToList();
        }

        public List<GrupoPermissao> ObterLista(string nome, int? codigo)
        {
            var query = from grupo in _context.GruposPermissao
                .Include("Permissoes")
                        select grupo;

            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(x => x.GrupoPermissaoNome == nome);
            }

            if (codigo != null && !codigo.Equals(0))
            {
                query = query.Where(x => x.GrupoPermissaoCodigo == codigo);
            }

            return query.ToList();
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
