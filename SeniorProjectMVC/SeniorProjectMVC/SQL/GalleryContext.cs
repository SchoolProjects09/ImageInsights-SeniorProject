using Microsoft.EntityFrameworkCore;

namespace SeniorProjectMVC.SQL
{
    public class GalleryContext : DbContext
    {
        public GalleryContext(DbContextOptions<GalleryContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<GalleryImage> Images { get; set; }
        public DbSet<SingleGalleryImage> SingleImages { get; set; }
        public DbSet<SQLResult> Results { get; set; }
    }
}
