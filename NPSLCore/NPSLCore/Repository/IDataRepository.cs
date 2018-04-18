

using NPSLCore.Models.DB;
using System.Collections.Generic;
namespace NPSLCore.Repository
{
    public interface IDataRepository<TEntity, U> where TEntity : class
    {
        void Add(TEntity item);
        IEnumerable<TEntity> GetAll();
        TEntity GetById(U UserId);
        U Delete(U UserId);
        void Update(TEntity item);
    }
}
