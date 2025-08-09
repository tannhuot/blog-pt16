using System.ComponentModel.DataAnnotations;

namespace blog_pt16.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
