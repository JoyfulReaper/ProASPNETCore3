using System.ComponentModel.DataAnnotations;

namespace Advanced.Models
{
    public class Department
    {
        public long DepartmentId { get; set; }

        [Required]
        public string? Name { get; set; }

        public IEnumerable<Person>? People { get; set; } = new HashSet<Person>();
    }
}
