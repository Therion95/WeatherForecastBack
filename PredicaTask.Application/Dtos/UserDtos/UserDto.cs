using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PredicaTask.Application.Dtos.UserDtos
{
    public class UserDto : BaseUserDto
    {
        [Required]
        public int Id { get; set; }
        [JsonIgnore]
        public string Hash { get; set; }
        
        
    }
}