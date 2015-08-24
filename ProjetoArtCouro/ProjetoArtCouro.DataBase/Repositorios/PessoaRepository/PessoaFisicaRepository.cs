using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.DataBase.Repositorios.PessoaRepository
{
    public class PessoaFisicaRepository : IPessoaFisicaRepository
    {
        private readonly DataBaseContext _context;

        public PessoaFisicaRepository(DataBaseContext context)
        {
            _context = context;
        }

        public PessoaFisica ObterPorId(Guid id)
        {
            return _context.PessoasFisicas.FirstOrDefault(x => x.PessoaId.Equals(id));
        }

        public List<PessoaFisica> ObterLista()
        {
            return _context.PessoasFisicas.ToList();
        }

        public List<PessoaFisica> ObterLista(int codigo, string nome, string cpf, string email)
        {
            var query = from pessoa in _context.PessoasFisicas
                .Include("Pessoa")
                .Include("Pessoa.MeiosComunicacao")
                .Include("Pessoa.Enderecos")
                select pessoa;

            if (!codigo.Equals(0))
            {
                query = query.Where(x => x.Pessoa.PessoaCodigo == codigo);
            }

            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(x => x.Pessoa.Nome == nome);
            }

            if (!string.IsNullOrEmpty(cpf))
            {
                query = query.Where(x => x.CPF == cpf);
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(x =>
                    x.Pessoa.MeiosComunicacao.Any(
                        a => a.TipoComunicacao == TipoComunicacaoEnum.Email && a.MeioComunicacaoNome == email));
            }

            return query.ToList();
        }

        public void Criar(PessoaFisica pessoaFisica)
        {
            _context.PessoasFisicas.Add(pessoaFisica);
            _context.SaveChanges();
        }

        public void Atualizar(PessoaFisica pessoaFisica)
        {
            _context.Entry(pessoaFisica).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(PessoaFisica pessoaFisica)
        {
            _context.PessoasFisicas.Remove(pessoaFisica);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
