using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.IProduto;
using ProjetoArtCouro.Domain.Models.Produtos;

namespace ProjetoArtCouro.DataBase.Repositorios.ProdutoRepository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly DataBaseContext _context;
        public ProdutoRepository(DataBaseContext context)
        {
            _context = context;
        }

        public Produto ObterPorId(Guid id)
        {
            return _context.Produtos.FirstOrDefault(x => x.ProdutoId.Equals(id));
        }

        public Produto ObterPorCodigo(int codigo)
        {
            return _context.Produtos.FirstOrDefault(x => x.ProdutoCodigo.Equals(codigo));
        }

        public Produto ObterComUnidadePorCodigo(int codigo)
        {
            return _context.Produtos.Include("Unidade").FirstOrDefault(x => x.ProdutoCodigo.Equals(codigo));
        }

        public List<Produto> ObterLista()
        {
            return _context.Produtos.ToList();
        }

        public List<Produto> ObterListaComUnidade()
        {
            return _context.Produtos.Include("Unidade").ToList();
        }

        public Produto Criar(Produto produto)
        {
            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return _context.Entry(produto).Entity;
        }

        public Produto Atualizar(Produto produto)
        {
            _context.Entry(produto).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
            return _context.Entry(produto).Entity;
        }

        public void Deletar(Produto produto)
        {
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
