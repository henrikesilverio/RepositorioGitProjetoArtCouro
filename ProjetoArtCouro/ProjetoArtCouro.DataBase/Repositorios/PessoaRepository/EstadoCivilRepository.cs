using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
using ProjetoArtCouro.Domain.Models.Pessoas;

namespace ProjetoArtCouro.DataBase.Repositorios.PessoaRepository
{
    public class EstadoCivilRepository : IEstadoCivilRepository
    {
        private readonly DataBaseContext _context;

        public EstadoCivilRepository(DataBaseContext context)
        {
            _context = context;
        }

        public EstadoCivil ObterPorId(Guid id)
        {
            return _context.EstadosCivis.FirstOrDefault(x => x.EstadoCivilId.Equals(id));
        }

        public EstadoCivil ObterPorCodigo(int codigo)
        {
            return _context.EstadosCivis.FirstOrDefault(x => x.EstadoCivilCodigo.Equals(codigo));
        }

        public List<EstadoCivil> ObterLista()
        {
            return _context.EstadosCivis.ToList();
        }

        public void Criar(EstadoCivil estadoCivil)
        {
            _context.EstadosCivis.Add(estadoCivil);
            _context.SaveChanges();
        }

        public void Atualizar(EstadoCivil estadoCivil)
        {
            _context.Entry(estadoCivil).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Deletar(EstadoCivil estadoCivil)
        {
            _context.EstadosCivis.Remove(estadoCivil);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
