using blog_pt16.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace blog_pt16.Pages.Post
{
    public class IndexModel(AppDBContext _db) : PageModel
    {
        public IEnumerable<PostModel> Posts { get; set; }

        public async Task OnGet()
        {
            Posts = await _db.Posts
                .Include(p => p.Category)
                .Include(p => p.Tags)
                .ToListAsync();
        }
    }
}
