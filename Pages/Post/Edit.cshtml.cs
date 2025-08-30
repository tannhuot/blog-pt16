using blog_pt16.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace blog_pt16.Pages.Post
{
    public class EditModel(AppDBContext _db, IWebHostEnvironment _environment) : PageModel
    {
        [BindProperty]
        public PostModel Post { get; set; }

        public SelectList CategoryItems { get; set; }
        [BindProperty]
        public int SelectedCategory { get; set; }


        public IEnumerable<TagModel> Tags { get; set; }
        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = [];


        public async Task OnGet(int id)
        {
            IEnumerable<CategoryModel> categories = await _db.Categories.ToListAsync();
            CategoryItems = new SelectList(categories,
                nameof(CategoryModel.Id),
                nameof(CategoryModel.Name));

            Tags = await _db.Tags.ToListAsync();

            Post = await _db.Posts
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            // Category <select>
            SelectedCategory = Post.CategoryModelId;

            // Tag Checkbox
            foreach (var selectedTag in Post.Tags)
            {
                foreach (var tag in Tags)
                {
                    if (tag.Id == selectedTag.Id)
                    {
                        tag.IsSelected = true;
                    }
                }
            }
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Remove("Post.Category");
            ModelState.Remove("Post.Thumnail");
            ModelState.Remove("Post.ImageFile");
            ModelState.Remove("Post.Tags");

            if (ModelState.IsValid)
            {
                var existPost = await _db.Posts
                    .Include(p => p.Category)
                    .Include(p => p.Tags)
                    .FirstAsync(p => p.Id == Post.Id);

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
                        //System.IO.File.Delete(filePath);
                        // Save file
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await Post.ImageFile.CopyToAsync(fileStream);
                        }
                        
                        // Delete old picture if exist
                        if (!String.IsNullOrEmpty(existPost.Thumnail))
                        {
                            var oldFile = Path.Combine(uploadFolder, existPost.Thumnail);
                            if (System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }
                        }

                        // assign new name
                        // Set file name to DB
                        existPost.Thumnail = uploadFileName;
                    }

                    existPost.Title = Post.Title;
                    existPost.Content = Post.Content;

                    if (String.IsNullOrEmpty(existPost.Thumnail))
                    {
                        existPost.Thumnail = "";
                    }

                    // one-to-many with tbl_Category
                    var cate = await _db.Categories.Include(c => c.Posts).FirstAsync(c => c.Id == SelectedCategory);
                    existPost.Category = cate;

                    //Many-to-Many
                    existPost.Tags = [];
                    foreach (var tagId in SelectedTagIds)
                    {
                        var existTag = await _db.Tags.FindAsync(tagId);
                        existPost.Tags.Add(existTag);
                    }

                    await _db.SaveChangesAsync();

                    return RedirectToPage("/post/index");
                }
            }

            await OnGet(Post.Id);
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