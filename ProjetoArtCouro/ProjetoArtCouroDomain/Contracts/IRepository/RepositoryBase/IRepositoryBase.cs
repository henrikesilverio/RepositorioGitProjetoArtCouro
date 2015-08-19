using System.Collections.Generic;

namespace ProjetoArtCouroDataBase.IRepositorios.RepositoryBase
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        // Definindo Operações padrões para todas as Repository que implementarem a Base
        void Add(TEntity obj);
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity obj);
        void Remove(TEntity obj);
        void Dispose();
    }
}
