using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PredicaTask.API.Helpers;
using PredicaTask.Application.Dtos.UserDtos;
using PredicaTask.Application.RepositoryInterfaces;
using PredicaTask.Application.Services;
using PredicaTask.Domain;

namespace PredicaTask.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;
        private readonly IUsersService _service;


        public AuthController(IMapper mapper, JwtService jwtService, IUsersService userService, IUnitOfWork uow)
        {
            _mapper = mapper;
            _jwtService = jwtService;
            _service = userService;
            _uow = uow;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            var userDto = await _service.RegisterAdmin(dto);

            return Ok(userDto);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _service.LoginAdmin(dto);
            
            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new
            {
                message = "success"
            });
        }

        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public new async Task<IActionResult> User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = await _uow.Users.GetById(userId);

                return Ok(user);
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "success"
            });
        }
    }
}