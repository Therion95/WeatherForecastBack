using System.Threading.Tasks;
using AutoMapper;
using PredicaTask.Application.Dtos.UserDtos;
using PredicaTask.Application.Exceptions;
using PredicaTask.Application.RepositoryInterfaces;
using PredicaTask.Domain;

namespace PredicaTask.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UsersService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<UserDto> RegisterAdmin(CreateUserDto createUser)
        {
            var userDto = new UserDto()
            {
                NickName = createUser.NickName,
                Email = createUser.Email,
                Hash = BCrypt.Net.BCrypt.HashPassword(createUser.Password),
            };

            var user = _mapper.Map<User>(userDto);

            await _uow.Users.Add(user);
            await _uow.Users.Save();

            return userDto;
        }

        public async Task<User> LoginAdmin(LoginDto loginDto)
        {
            var user = await ReturnObjectIfTakenDataFromDbIsNotNull(loginDto);
            return user;
        }
        
        private async Task<User> ReturnObjectIfTakenDataFromDbIsNotNull(LoginDto loginDto)
        {    
            var user = await _uow.Users.GetByEmail(loginDto.Email);
            
            if (user == null)
            {
                throw new NotFoundException("Admin not found");
            }
            return user;
        }
    }
}