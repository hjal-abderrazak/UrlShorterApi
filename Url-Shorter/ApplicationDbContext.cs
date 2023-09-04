using Microsoft.EntityFrameworkCore;
using Url_Shorter.Entities;
using Url_Shorter.Services;

namespace Url_Shorter
{
    public class ApplicationDbContext:DbContext
    {

        public ApplicationDbContext(DbContextOptions options):base(options)
        {
        }

        public DbSet<ShortenUrl> ShortenUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ShortenUrl>(modelBuilder =>
            {
                modelBuilder.Property(s=>s.Code).HasMaxLength(UrlShorteningService.NumberOfCharsInShortLink);
                modelBuilder.HasIndex(s=>s.Code).IsUnique();

            });
        }
    }
}
