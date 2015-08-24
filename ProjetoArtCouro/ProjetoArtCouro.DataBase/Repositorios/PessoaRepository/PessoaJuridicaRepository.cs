using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
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

        public List<PessoaJuridica> ObterLista()
        {
            return _context.PessoasJuridicas.ToList();
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
