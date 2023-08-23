using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models.MainDb;

namespace Portfolio.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<BlogLikeUpdate> BlogLikeUpdates { get; set; }

        public DbSet<RelatedFile> RelatedFiles { get; set; }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }
    }
}