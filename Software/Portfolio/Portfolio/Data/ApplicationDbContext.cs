using Microsoft.EntityFrameworkCore;
using Portfolio.Models.MainDb;

namespace Portfolio.Data
{
    /// <summary>
    /// This ApplicationDbContext provides a database context object used to track user actions and data to display.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public DbSet<BlogLikeUpdate> BlogLikeUpdates { get; set; }

        public DbSet<RelatedFile> RelatedFiles { get; set; }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}