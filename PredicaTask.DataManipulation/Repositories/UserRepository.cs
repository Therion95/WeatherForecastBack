using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PredicaTask.Application.RepositoryInterfaces;
using PredicaTask.Data;
using PredicaTask.Domain;

namespace PredicaTask.DataManipulation.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PredicaTaskDbContext _context;
        private readonly DbSet<User> _db;
        
        public UserRepository(PredicaTaskDbContext context)
        {
            _context = context;
            _db = context.Set<User>();
        }
        
        public async Task Add(User entity)
        {
            await _db.AddAsync(entity);
        }
        
        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}