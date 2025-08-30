using blog_pt16.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace blog_pt16.Pages.Post
{
    public class CreateModel(AppDBContext _db, IWebHostEnvironment _environment) : PageModel
    {
        [BindProperty]
        public PostModel Post { get; set; }

        public SelectList CategoryItems { get; set; }
        [BindProperty]
        public int SelectedCategory { get; set; }

        
        public IEnumerable<TagModel> Tags { get; set; }
        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = [];


        public async Task OnGet()
        {
            IEnumerable<CategoryModel> categories = await _db.Categories.ToListAsync();
            CategoryItems = new SelectList(categories,
                nameof(CategoryModel.Id), 
                nameof(CategoryModel.Name));

            Tags = await _db.Tags.ToListAsync();
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Remove("Post.Category");
            ModelState.Remove("Post.Thumnail");
            ModelState.Remove("Post.ImageFile");
            ModelState.Remove("Post.Tags");

            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    if (Post.ImageFile != null && Post.ImageFile.Length > 0)
                    {
                        // Get the directory "wwwroot"
                        var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads");

                        // Create folder "uploads" if not exist
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        // Create unique file name
                        var fileName = Path.GetFileNameWithoutExtension(Post.ImageFile.FileName);
                        var extension = Path.GetExtension(Post.ImageFile.FileName);
                        var uploadFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                        var filePath = Path.Combine(uploadFolder, uploadFileName);

                        // Save file
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await Post.ImageFile.CopyToAsync(fileStream);
                        }
                        // Set file name to DB
                        Post.Thumnail = uploadFileName;
                    } else
                    {
                        Post.Thumnail = "";
                    }

                    // one-to-many with tbl_Category
                    var cate = await _db.Categories.Include(c => c.Posts).FirstAsync(c => c.Id == SelectedCategory);
                    cate.Posts.Add(Post);
                    // Post.Category = cate;

                    //Many-to-Many
                    Post.Tags = [];
                    foreach (var tagId in SelectedTagIds)
                    {
                        var existTag = await _db.Tags.FindAsync(tagId);
                        Post.Tags.Add(existTag);
                    }

                    await _db.Posts.AddAsync(Post);

                    _db.SaveChanges();

                    return RedirectToPage("/post/index");
                }
            }

            await OnGet();
            foreach (var id in SelectedTagIds)
            {
                foreach (var tag in Tags)
                {
                    if (tag.Id == id)
                    {
                        tag.IsSelected = true;
                    }
                }
            }
            return Page();
        }
    }
}
