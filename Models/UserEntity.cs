using Microsoft.AspNetCore.Identity;

namespace blog_pt16.Models
{
    public class UserEntity : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
