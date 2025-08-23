using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blog_pt16.Models
{
    public class TagModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; } = false;

        // Many-To-Many
        // Navigation Reference Property
        public ICollection<PostModel> Posts { get; set; }
    }
}
