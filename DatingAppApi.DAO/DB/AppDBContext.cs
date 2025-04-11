using DatingAppApi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.DAL.DB
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUsers> AppUsers { get; set; }
        public DbSet<UserLikes> UserLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLikes>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserId });

            builder.Entity<UserLikes>()
                .HasOne(s => s.SourceUser)
                .WithMany(l=> l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLikes>()
                .HasOne(s => s.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
