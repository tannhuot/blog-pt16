using blog_pt16.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace blog_pt16.Pages.User
{
    public class IndexModel(UserManager<UserEntity> _user) : PageModel
    {
        public UserEntity appUser;

        public async Task OnGet()
        {
            var task = _user.GetUserAsync(User);
            task.Wait();
            appUser = task.Result;

            await _user.AddToRoleAsync(appUser, "Admin");

            var isAdmin = await _user.IsInRoleAsync(appUser, "Admin");
        }
    }
}
