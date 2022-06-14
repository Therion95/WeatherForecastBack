using System.Threading.Tasks;
using PredicaTask.Application.RepositoryInterfaces;
using PredicaTask.Data;
using PredicaTask.DataManipulation.Repositories;

namespace PredicaTask.DataManipulation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PredicaTaskDbContext _context;

        public UnitOfWork(PredicaTaskDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => new UserRepository(_context);
        public IWeatherRepository Weathers => new WeatherRepository(_context);
        
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}