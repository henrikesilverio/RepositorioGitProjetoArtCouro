using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.ICompra;
using ProjetoArtCouro.Domain.Models.Compras;

namespace ProjetoArtCouro.DataBase.Repositorios.CompraRepository
{
    public class ItemCompraRepository : IItemCompraRepository
    {
        private readonly DataBaseContext _context;
        public ItemCompraRepository(DataBaseContext context)
        {
            _context = context;
        }

        public ItemCompra ObterPorId(Guid id)
        {
            return _context.ItensCompra.FirstOrDefault(x => x.ItemCompraId.Equals(id));
        }

        public ItemCompra ObterPorCodigo(int codigo)
        {
            return _context.ItensCompra.FirstOrDefault(x => x.ItemCompraCodigo.Equals(codigo));
        }

        public List<ItemCompra> ObterLista()
        {
            return _context.ItensCompra.ToList();
        }

        public ItemCompra Criar(ItemCompra itemCompra)
        {
            _context.ItensCompra.Add(itemCompra);
            _context.SaveChanges();
            return _context.Entry(itemCompra).Entity;
        }

        public void Atualizar(ItemCompra itemCompra)
        {
            _context.Entry(itemCompra).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(ItemCompra itemCompra)
        {
            _context.ItensCompra.Remove(itemCompra);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
