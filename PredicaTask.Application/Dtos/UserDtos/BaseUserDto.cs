using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PredicaTask.Application.Dtos.UserDtos
{
    public class BaseUserDto
    {
        [Required]
        public string NickName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [JsonIgnore]
        public bool Admin { get; set; }
    }
}