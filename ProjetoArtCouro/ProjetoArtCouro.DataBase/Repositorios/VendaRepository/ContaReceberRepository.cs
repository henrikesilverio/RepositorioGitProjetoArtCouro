using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.IVenda;
using ProjetoArtCouro.Domain.Models.Vendas;

namespace ProjetoArtCouro.DataBase.Repositorios.VendaRepository
{
    public class ContaReceberRepository : IContaReceberRepository
    {
        private readonly DataBaseContext _context;
        public ContaReceberRepository(DataBaseContext context)
        {
            _context = context;
        }

        public ContaReceber ObterPorId(Guid id)
        {
            return _context.ContasReceber.FirstOrDefault(x => x.ContaReceberId.Equals(id));
        }

        public ContaReceber ObterPorCodigo(int codigo)
        {
            return _context.ContasReceber.FirstOrDefault(x => x.ContaReceberCodigo.Equals(codigo));
        }

        public ContaReceber ObterPorCodigoComVenda(int codigo)
        {
            return _context.ContasReceber.Include("Venda").FirstOrDefault(x => x.ContaReceberCodigo.Equals(codigo));
        }

        public List<ContaReceber> ObterLista()
        {
            return _context.ContasReceber.ToList();
        }

        public List<ContaReceber> ObterListaPorCodigoVenda(int codigoVenda)
        {
            return _context.ContasReceber.Include("Venda").Where(x => x.Venda.VendaCodigo.Equals(codigoVenda)).ToList();
        }

        public List<ContaReceber> ObterLista(int codigoVenda, int codigoCliente, DateTime dataEmissao, DateTime dataVencimento,
            int statusContaReceber, string nomeCliente, string documento, int codigoUsuario)
        {
            var query = from contaReceber in _context.ContasReceber
                .Include("Venda")
                .Include("Venda.Usuario")
                .Include("Venda.Cliente.PessoaFisica")
                .Include("Venda.Cliente.PessoaJuridica")
                        select contaReceber;

            if (!codigoVenda.Equals(0))
            {
                query = query.Where(x => x.Venda.VendaCodigo == codigoVenda);
            }

            if (!codigoCliente.Equals(0))
            {
                query = query.Where(x => x.Venda.Cliente.PessoaCodigo == codigoCliente);
            }

            if (!dataEmissao.Equals(new DateTime()))
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.Venda.DataCadastro) == dataEmissao.Date);
            }

            if (!dataVencimento.Equals(new DateTime()))
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.DataVencimento) == dataVencimento.Date);
            }

            if (!statusContaReceber.Equals(0))
            {
                query = query.Where(x => (int)x.StatusContaReceber == statusContaReceber);
            }

            if (!string.IsNullOrEmpty(nomeCliente))
            {
                query = query.Where(x => x.Venda.Cliente.Nome == nomeCliente);
            }

            if (!string.IsNullOrEmpty(documento))
            {
                query = query.Where(x => x.Venda.Cliente.PessoaFisica.CPF == documento || x.Venda.Cliente.PessoaJuridica.CNPJ == documento);
            }

            if (!codigoUsuario.Equals(0))
            {
                query = query.Where(x => x.Venda.Usuario.UsuarioCodigo == codigoUsuario);
            }

            return query.ToList();
        }

        public void Criar(ContaReceber contaReceber)
        {
            _context.ContasReceber.Add(contaReceber);
            _context.SaveChanges();
        }

        public void Atualizar(ContaReceber contaReceber)
        {
            _context.Entry(contaReceber).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(ContaReceber contaReceber)
        {
            _context.ContasReceber.Remove(contaReceber);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
