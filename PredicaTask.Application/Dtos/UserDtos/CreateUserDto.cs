using System.ComponentModel.DataAnnotations;

namespace PredicaTask.Application.Dtos.UserDtos
{
    public class CreateUserDto : BaseUserDto
    {
        [Required]
        public string Password { get; set; }
    }
}