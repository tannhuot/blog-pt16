using System.ComponentModel.DataAnnotations.Schema;

namespace blog_pt16.Models.Utils
{
    [NotMapped]
    public class Utils : IUtils
    {
        public string getImage(string name)
        {
            if (string.IsNullOrEmpty(name)) return "/images/img1.png";
            return $"/uploads/{name}";
        }
    }
}
