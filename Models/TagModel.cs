using System.ComponentModel.DataAnnotations;

namespace blog_pt16.Models
{
    public class TagModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Many-To-Many
        // Navigation Reference Property
        public ICollection<PostModel> Posts { get; set; }
    }
}
