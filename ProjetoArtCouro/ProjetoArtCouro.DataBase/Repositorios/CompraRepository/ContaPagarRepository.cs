using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.ICompra;
using ProjetoArtCouro.Domain.Models.Compras;

namespace ProjetoArtCouro.DataBase.Repositorios.CompraRepository
{
    public class ContaPagarRepository : IContaPagarRepository
    {
        private readonly DataBaseContext _context;
        public ContaPagarRepository(DataBaseContext context)
        {
            _context = context;
        }

        public ContaPagar ObterPorId(Guid id)
        {
            return _context.ContasPagar.FirstOrDefault(x => x.ContaPagarId.Equals(id));
        }

        public ContaPagar ObterPorCodigo(int codigo)
        {
            return _context.ContasPagar.FirstOrDefault(x => x.ContaPagarCodigo.Equals(codigo));
        }

        public ContaPagar ObterPorCodigoComCompra(int codigo)
        {
            return _context.ContasPagar.Include("Compra").FirstOrDefault(x => x.ContaPagarCodigo.Equals(codigo));
        }

        public List<ContaPagar> ObterLista()
        {
            return _context.ContasPagar.ToList();
        }

        public List<ContaPagar> ObterListaPorCodigoCompra(int codigoCompra)
        {
            return
                _context.ContasPagar.Include("Compra").Where(x => x.Compra.CompraCodigo.Equals(codigoCompra)).ToList();
        }

        public List<ContaPagar> ObterLista(int codigoCompra, int codigoFornecedor, DateTime dataEmissao, DateTime dataVencimento,
            int statusContaPagar, string nomeFornecedor, string documento, int codigoUsuario)
        {
            var query = from contaPagar in _context.ContasPagar
                .Include("Compra")
                .Include("Compra.Usuario")
                .Include("Compra.Fornecedor.PessoaFisica")
                .Include("Compra.Fornecedor.PessoaJuridica")
                        select contaPagar;

            if (!codigoCompra.Equals(0))
            {
                query = query.Where(x => x.Compra.CompraCodigo == codigoCompra);
            }

            if (!codigoFornecedor.Equals(0))
            {
                query = query.Where(x => x.Compra.Fornecedor.PessoaCodigo == codigoFornecedor);
            }

            if (!dataEmissao.Equals(new DateTime()))
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.Compra.DataCadastro) == dataEmissao.Date);
            }

            if (!dataVencimento.Equals(new DateTime()))
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.DataVencimento) == dataVencimento.Date);
            }

            if (!statusContaPagar.Equals(0))
            {
                query = query.Where(x => (int)x.StatusContaPagar == statusContaPagar);
            }

            if (!string.IsNullOrEmpty(nomeFornecedor))
            {
                query = query.Where(x => x.Compra.Fornecedor.Nome == nomeFornecedor);
            }

            if (!string.IsNullOrEmpty(documento))
            {
                query = query.Where(x => x.Compra.Fornecedor.PessoaFisica.CPF == documento || x.Compra.Fornecedor.PessoaJuridica.CNPJ == documento);
            }

            if (!codigoUsuario.Equals(0))
            {
                query = query.Where(x => x.Compra.Usuario.UsuarioCodigo == codigoUsuario);
            }

            return query.ToList();
        }

        public void Criar(ContaPagar contaPagar)
        {
            _context.ContasPagar.Add(contaPagar);
            _context.SaveChanges();
        }

        public void Atualizar(ContaPagar contaPagar)
        {
            _context.Entry(contaPagar).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(ContaPagar contaPagar)
        {
            _context.ContasPagar.Remove(contaPagar);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
