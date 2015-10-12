using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.DataBase.Repositorios.PessoaRepository
{
    public class PessoaJuridicaRepository : IPessoaJuridicaRepository
    {
        private readonly DataBaseContext _context;

        public PessoaJuridicaRepository(DataBaseContext context)
        {
            _context = context;
        }

        public PessoaJuridica ObterPorId(Guid id)
        {
            return _context.PessoasJuridicas.FirstOrDefault(x => x.PessoaId.Equals(id));
        }

        public PessoaJuridica ObterPorCNPJ(string cnpj)
        {
            return _context.PessoasJuridicas.FirstOrDefault(x => x.CNPJ.Equals(cnpj));
        }

        public List<PessoaJuridica> ObterLista()
        {
            return _context.PessoasJuridicas.ToList();
        }

        public List<PessoaJuridica> ObterLista(int codigo, string nome, string cnpj, string email, TipoPapelPessoaEnum papelCodigo)
        {
            var query = from pessoa in _context.PessoasJuridicas
                .Include("Pessoa")
                .Include("Pessoa.Papeis")
                .Include("Pessoa.MeiosComunicacao")
                .Include("Pessoa.Enderecos")
                        select pessoa;

            if (!codigo.Equals(0))
            {
                query = query.Where(x => x.Pessoa.PessoaCodigo == codigo);
            }

            if (papelCodigo != TipoPapelPessoaEnum.Nenhum)
            {
                query = query.Where(x => x.Pessoa.Papeis.Any(a => a.PapelCodigo == (int)papelCodigo));
            }

            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(x => x.Pessoa.Nome == nome);
            }

            if (!string.IsNullOrEmpty(cnpj))
            {
                query = query.Where(x => x.CNPJ == cnpj);
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(x =>
                    x.Pessoa.MeiosComunicacao.Any(
                        a => a.TipoComunicacao == TipoComunicacaoEnum.Email && a.MeioComunicacaoNome == email));
            }

            return query.ToList();
        }

        public void Criar(PessoaJuridica pessoaJuridica)
        {
            _context.PessoasJuridicas.Add(pessoaJuridica);
            _context.SaveChanges();
        }

        public void Atualizar(PessoaJuridica pessoaJuridica)
        {
            _context.Entry(pessoaJuridica).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(PessoaJuridica pessoaJuridica)
        {
            _context.PessoasJuridicas.Remove(pessoaJuridica);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
