using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.ICompra;
using ProjetoArtCouro.Domain.Models.Compras;

namespace ProjetoArtCouro.DataBase.Repositorios.CompraRepository
{
    public class CompraRepository : ICompraRepository
    {
        private readonly DataBaseContext _context;
        public CompraRepository(DataBaseContext context)
        {
            _context = context;
        }

        public Compra ObterPorId(Guid id)
        {
            return _context.Compras.FirstOrDefault(x => x.CompraId.Equals(id));
        }

        public Compra ObterPorCodigo(int codigo)
        {
            return _context.Compras.FirstOrDefault(x => x.CompraCodigo.Equals(codigo));
        }

        public Compra ObterPorCodigoComItensCompra(int codigo)
        {
            return _context.Compras.Include("ItensCompra").FirstOrDefault(x => x.CompraCodigo.Equals(codigo));
        }

        public List<Compra> ObterLista()
        {
            return _context.Compras.ToList();
        }

        public List<Compra> ObterLista(int codigoCompra, int codigoFornecedor, DateTime dataCadastro, int statusCompra, string nomeFornecedor,
            string documento, int codigoUsuario)
        {
            var query = from compra in _context.Compras
                .Include("Usuario")
                .Include("ItensCompra")
                .Include("Fornecedor")
                .Include("Fornecedor.PessoaFisica")
                .Include("Fornecedor.PessoaJuridica")
                        select compra;

            if (!codigoCompra.Equals(0))
            {
                query = query.Where(x => x.CompraCodigo == codigoCompra);
            }

            if (!codigoFornecedor.Equals(0))
            {
                query = query.Where(x => x.Fornecedor.PessoaCodigo == codigoFornecedor);
            }

            if (!dataCadastro.Equals(new DateTime()))
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.DataCadastro) == dataCadastro.Date);
            }

            if (!statusCompra.Equals(0))
            {
                query = query.Where(x => (int)x.StatusCompra == statusCompra);
            }

            if (!string.IsNullOrEmpty(nomeFornecedor))
            {
                query = query.Where(x => x.Fornecedor.Nome == nomeFornecedor);
            }

            if (!string.IsNullOrEmpty(documento))
            {
                query = query.Where(x => x.Fornecedor.PessoaFisica.CPF == documento || x.Fornecedor.PessoaJuridica.CNPJ == documento);
            }

            if (!codigoUsuario.Equals(0))
            {
                query = query.Where(x => x.Usuario.UsuarioCodigo == codigoUsuario);
            }

            return query.ToList();
        }

        public void Criar(Compra compra)
        {
            _context.Compras.Add(compra);
            _context.SaveChanges();
        }

        public void Atualizar(Compra compra)
        {
            _context.Entry(compra).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(Compra compra)
        {
            _context.Compras.Remove(compra);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
