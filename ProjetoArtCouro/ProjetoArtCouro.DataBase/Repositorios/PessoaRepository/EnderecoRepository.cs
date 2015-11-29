using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.DataBase.Repositorios.PessoaRepository
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly DataBaseContext _context;

        public EnderecoRepository(DataBaseContext context)
        {
            _context = context;
        }

        public Endereco ObterPorId(Guid id)
        {
            return _context.Enderecos.FirstOrDefault(x => x.EnderecoId.Equals(id));
        }

        public List<Endereco> ObterLista()
        {
            return _context.Enderecos.ToList();
        }

        public Endereco Criar(Endereco endereco)
        {
            _context.Enderecos.Add(endereco);
            _context.SaveChanges();
            return _context.Entry(endereco).Entity;
        }

        public void Atualizar(Endereco endereco)
        {
            _context.Entry(endereco).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(Endereco endereco)
        {
            _context.Enderecos.Remove(endereco);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
