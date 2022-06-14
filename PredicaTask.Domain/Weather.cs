using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PredicaTask.Domain
{
    public class Weather
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName="date")]
        public DateTime Date { get; set; }
        [Required]
        public int TemperatureC { get; set; }
        //public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
        public string Summary { get; set; }
        [Required]
        public string City { get; set; }
    }
}