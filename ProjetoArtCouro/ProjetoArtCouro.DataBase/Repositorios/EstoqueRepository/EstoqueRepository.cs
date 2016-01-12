using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.IEstoque;
using ProjetoArtCouro.Domain.Models.Estoques;

namespace ProjetoArtCouro.DataBase.Repositorios.EstoqueRepository
{
    public class EstoqueRepository : IEstoqueRepository
    {
        private readonly DataBaseContext _context; 
        public EstoqueRepository(DataBaseContext context)
        {
            _context = context;
        }

        public Estoque ObterPorId(Guid id)
        {
            return _context.Estoques.FirstOrDefault(x => x.EstoqueId.Equals(id));
        }

        public Estoque ObterPorCodigo(int codigo)
        {
            return _context.Estoques.FirstOrDefault(x => x.EstoqueCodigo.Equals(codigo));
        }

        public List<Estoque> ObterLista()
        {
            return _context.Estoques.ToList();
        }

        public void Criar(Estoque estoque)
        {
            _context.Estoques.Add(estoque);
            _context.SaveChanges();
        }

        public void Atualizar(Estoque estoque)
        {
            _context.Entry(estoque).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(Estoque estoque)
        {
            _context.Estoques.Remove(estoque);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
