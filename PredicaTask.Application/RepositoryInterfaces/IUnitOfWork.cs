using System.Threading.Tasks;

namespace PredicaTask.Application.RepositoryInterfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IWeatherRepository Weathers { get; }
        Task Save();
    }
}