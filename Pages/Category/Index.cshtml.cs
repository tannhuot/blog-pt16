using blog_pt16.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace blog_pt16.Pages.Category
{
    public class IndexModel(AppDBContext _db) : PageModel
    {

        public IEnumerable<CategoryModel> Categories { get; set; }
        public async Task OnGet()

        {
            Categories = await _db.Categories.ToListAsync();
        }
    }
}
