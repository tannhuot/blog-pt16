using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace blog_pt16.Pages.User
{
    public class CreateRoleModel(RoleManager<IdentityRole> _roleManager) : PageModel
    {

        [BindProperty]
        public string Name { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {

            var isExist = await _roleManager.RoleExistsAsync(Name);
            if (!isExist)
            {
                var newRole = new IdentityRole();
                newRole.Name = Name;
                newRole.NormalizedName = Name;
                var result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    Console.WriteLine("Success");
                }
                else
                {
                    Console.WriteLine("Failed");
                    foreach (var err in result.Errors)
                    {
                        Console.WriteLine($"Error: {err}");
                    }
                }
            }
            return RedirectToPage("/Index");
        }
    }
}
