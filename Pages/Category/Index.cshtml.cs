using blog_pt16.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace blog_pt16.Pages.Category
{
    public class IndexModel(AppDBContext _db) : PageModel
    {

        public IEnumerable<CategoryModel> Categories { get; set; }

        [BindProperty]
        public string CategoryName { get; set; }
        public async Task OnGet()

        {
            Categories = await _db.Categories.ToListAsync();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            var cate = await _db.Categories.FindAsync(id);
            _db.Categories.Remove(cate);
            await _db.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSearch()
        {
            Categories = await _db.Categories.Where(c => c.Name.Contains(CategoryName)).ToListAsync();
            return Page();
        }
    }
}
