using blog_pt16.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace blog_pt16.Pages.Category
{
    public class CreateModel(AppDBContext _db) : PageModel
    {
        [BindProperty]
        public CategoryModel CategoryModel { get; set; }

        public string isNameError { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost() {

            if (ModelState.IsValid)
            {
                await _db.Categories.AddAsync(CategoryModel);
                await _db.SaveChangesAsync();
                return RedirectToPage("/category/index");
            }

            if (CategoryModel.Name.IsNullOrEmpty())
            {
                isNameError = "is-invalid";
            }
            else
            {
                isNameError = "";
            }
            return Page();
        }
    }
}
