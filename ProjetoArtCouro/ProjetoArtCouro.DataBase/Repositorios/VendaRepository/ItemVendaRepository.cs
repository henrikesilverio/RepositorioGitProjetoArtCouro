using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.IVenda;
using ProjetoArtCouro.Domain.Models.Vendas;

namespace ProjetoArtCouro.DataBase.Repositorios.VendaRepository
{
    public class ItemVendaRepository : IItemVendaRepository
    {
        private readonly DataBaseContext _context;
        public ItemVendaRepository(DataBaseContext context)
        {
            _context = context;
        }

        public ItemVenda ObterPorId(Guid id)
        {
            return _context.ItensVenda.FirstOrDefault(x => x.ItemVendaId.Equals(id));
        }

        public ItemVenda ObterPorCodigo(int codigo)
        {
            return _context.ItensVenda.FirstOrDefault(x => x.ItemVendaCodigo.Equals(codigo));
        }

        public List<ItemVenda> ObterLista()
        {
            return _context.ItensVenda.ToList();
        }

        public ItemVenda Criar(ItemVenda itemVenda)
        {
            _context.ItensVenda.Add(itemVenda);
            _context.SaveChanges();
            return _context.Entry(itemVenda).Entity;
        }

        public void Atualizar(ItemVenda itemVenda)
        {
            _context.Entry(itemVenda).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(ItemVenda itemVenda)
        {
            _context.ItensVenda.Remove(itemVenda);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
