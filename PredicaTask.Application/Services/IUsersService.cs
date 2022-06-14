using System.Threading.Tasks;
using PredicaTask.Application.Dtos.UserDtos;
using PredicaTask.Domain;

namespace PredicaTask.Application.Services
{
    public interface IUsersService
    {
        Task<UserDto> RegisterAdmin(CreateUserDto createUser);
        Task<User> LoginAdmin(LoginDto loginDto);
    }
}