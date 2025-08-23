using System.ComponentModel.DataAnnotations.Schema;

namespace blog_pt16.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Thumnail { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        // One-To-Many (Many)
        // FK properties
        public int CategoryModelId { get; set; }
        // Navigation Reference Property
        public CategoryModel Category { get; set; }

        // Many-To-Many
        // Navigation Reference Property
        public ICollection<TagModel> Tags { get; set; }
    }
}
