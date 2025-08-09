using Microsoft.EntityFrameworkCore;

namespace blog_pt16.Models
{
    public class AppDBContext(IConfiguration configuration) : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("conn"));
        }

        public DbSet<CategoryModel> Categories { get; set; }
    }
}
