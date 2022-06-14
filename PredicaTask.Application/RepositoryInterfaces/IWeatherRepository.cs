using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PredicaTask.Domain;

namespace PredicaTask.Application.RepositoryInterfaces
{
    public interface IWeatherRepository
    {
        Task Add(Weather entity);
        Task<Weather> Get(Expression<Func<Weather, bool>> expression = null, List<string> includes = null);
        Task<IReadOnlyCollection<Weather>> GetAll(Expression<Func<Weather, bool>> expression = null, Func<IQueryable<Weather>, IOrderedQueryable<Weather>> orderBy = null, List<string> includes = null);
        Task Modify(Weather entity);
        Task Remove(int id);
        Task Save();
    }
}