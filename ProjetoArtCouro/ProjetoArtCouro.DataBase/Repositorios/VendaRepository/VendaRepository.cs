using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.IVenda;
using ProjetoArtCouro.Domain.Models.Vendas;

namespace ProjetoArtCouro.DataBase.Repositorios.VendaRepository
{
    public class VendaRepository : IVendaRepository
    {
        private readonly DataBaseContext _context;
        public VendaRepository(DataBaseContext context)
        {
            _context = context;
        }

        public Venda ObterPorId(Guid id)
        {
            return _context.Vendas.FirstOrDefault(x => x.VendaId.Equals(id));
        }

        public Venda ObterPorCodigo(int codigo)
        {
            return _context.Vendas.FirstOrDefault(x => x.VendaCodigo.Equals(codigo));
        }

        public Venda ObterPorCodigoComItensVenda(int codigo)
        {
            return _context.Vendas.Include("ItensVenda").FirstOrDefault(x => x.VendaCodigo.Equals(codigo));
        }

        public List<Venda> ObterLista()
        {
            return _context.Vendas.ToList();
        }

        public List<Venda> ObterLista(int codigoVenda, int codigoCliente, DateTime dataCadastro, int statusVenda,
            string nomeCliente, string documento, int codigoUsuario)
        {
            var query = from venda in _context.Vendas
                .Include("Usuario")
                .Include("ItensVenda")
                .Include("Cliente")
                .Include("Cliente.PessoaFisica")
                .Include("Cliente.PessoaJuridica")
                select venda;

            if (!codigoVenda.Equals(0))
            {
                query = query.Where(x => x.VendaCodigo == codigoVenda);
            }

            if (!codigoCliente.Equals(0))
            {
                query = query.Where(x => x.Cliente.PessoaCodigo == codigoCliente);
            }

            if (!dataCadastro.Equals(new DateTime()))
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.DataCadastro) == dataCadastro.Date);
            }

            if (!statusVenda.Equals(0))
            {
                query = query.Where(x => (int)x.StatusVenda == statusVenda);
            }

            if (!string.IsNullOrEmpty(nomeCliente))
            {
                query = query.Where(x => x.Cliente.Nome == nomeCliente);
            }

            if (!string.IsNullOrEmpty(documento))
            {
                query = query.Where(x => x.Cliente.PessoaFisica.CPF == documento || x.Cliente.PessoaJuridica.CNPJ == documento);
            }

            if (!codigoUsuario.Equals(0))
            {
                query = query.Where(x => x.Usuario.UsuarioCodigo == codigoUsuario);
            }

            return query.ToList();
        }

        public void Criar(Venda venda)
        {
            _context.Vendas.Add(venda);
            _context.SaveChanges();
        }

        public void Atualizar(Venda venda)
        {
            _context.Entry(venda).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(Venda venda)
        {
            _context.Vendas.Remove(venda);
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
