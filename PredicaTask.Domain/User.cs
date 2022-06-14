using System.ComponentModel.DataAnnotations;

namespace PredicaTask.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Hash { get; set; }
        
        public bool? Admin { get; set; }
    }
}