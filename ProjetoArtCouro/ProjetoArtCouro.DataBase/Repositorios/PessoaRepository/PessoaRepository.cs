using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.DataBase.Repositorios.PessoaRepository
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly DataBaseContext _context;

        public PessoaRepository(DataBaseContext context)
        {
            _context = context;
        }

        public Pessoa ObterPorId(Guid id)
        {
            return _context.Pessoas.FirstOrDefault(x => x.PessoaId.Equals(id));
        }

        public Pessoa ObterPorCodigo(int codigo)
        {
            return _context.Pessoas.FirstOrDefault(x => x.PessoaCodigo.Equals(codigo));
        }

        public Pessoa ObterPorCodigoComPessoaCompleta(int codigo)
        {
            return _context.Pessoas
                .Include("PessoaFisica")
                .Include("PessoaFisica.EstadoCivil")
                .Include("PessoaJuridica")
                .Include("Papeis")
                .Include("MeiosComunicacao")
                .Include("Enderecos")
                .Include("Enderecos.Estado")
                .FirstOrDefault(x => x.PessoaCodigo.Equals(codigo));
        }

        public List<Pessoa> ObterLista()
        {
            return _context.Pessoas.ToList();
        }

        public void Criar(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            _context.SaveChanges();
        }

        public void Atualizar(Pessoa pessoa)
        {
            _context.Entry(pessoa).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(Pessoa pessoa)
        {
            _context.Pessoas.Remove(pessoa);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
