using System.Threading.Tasks;
using PredicaTask.Domain;

namespace PredicaTask.Application.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task<User> GetById(int id);
        Task Add(User entity);
        Task Save();
    }
}