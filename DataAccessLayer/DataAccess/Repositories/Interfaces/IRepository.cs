using DataAccessLayer.Entities.Interfaces;

namespace DataAccessLayer.DataAccess.Repositories.Interfaces;

public interface IRepository<T> where T : IEntity
{
    /// <summary>
    ///     Get all the entities
    /// </summary>
    /// <returns>A queryable of all the entities</returns>
    /// <remarks>Does not execute the query</remarks>
    IQueryable<T> QueryMany();
}