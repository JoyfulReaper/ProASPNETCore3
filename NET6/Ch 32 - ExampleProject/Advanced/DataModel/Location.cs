using System.ComponentModel.DataAnnotations;

namespace Advanced.Models
{
    public class Location
    {
        public long LocationId { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? State { get; set; }


        public IEnumerable<Person>? People { get; set; } = new HashSet<Person>();
    }
}
