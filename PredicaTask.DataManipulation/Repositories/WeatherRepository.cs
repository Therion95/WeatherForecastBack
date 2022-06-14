using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PredicaTask.Application.RepositoryInterfaces;
using PredicaTask.Data;
using PredicaTask.Domain;

namespace PredicaTask.DataManipulation.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly PredicaTaskDbContext _context;
        private readonly DbSet<Weather> _db;
        
        public WeatherRepository(PredicaTaskDbContext context)
        {
            _context = context;
            _db = context.Set<Weather>();
        }

        public async Task<IReadOnlyCollection<Weather>> GetAll(Expression<Func<Weather, bool>> expression = null,
            Func<IQueryable<Weather>, IOrderedQueryable<Weather>> orderBy = null,
            List<string> includes = null)
        {
            IQueryable<Weather> query = _db;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Weather> Get(Expression<Func<Weather, bool>> expression = null,
            List<string> includes = null)
        {
            IQueryable<Weather> query = _db;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task Add(Weather entity)
        {
            await _db.AddAsync(entity);
        }

        public async Task Remove(int id)
        {
            var entity = await _db.FindAsync(id);
            _db.Remove(entity);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Modify(Weather entity)
        {
             _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}