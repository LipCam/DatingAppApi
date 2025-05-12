using DatingAppApi.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.DAL.DB
{
    public class AppDBContext : IdentityDbContext<AppUsers, AppRole, long, 
        IdentityUserClaim<long>, AppUserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, 
        IdentityUserToken<long>> //DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        //public DbSet<AppUsers> AppUsers { get; set; }
        public DbSet<UserLikes> UserLikes { get; set; }
        public DbSet<Messages> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUsers>()
                .HasMany(s => s.UserRoles)
                .WithOne(l => l.User)
                .HasForeignKey(s => s.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(s => s.UserRoles)
                .WithOne(l => l.Role)
                .HasForeignKey(s => s.RoleId)
                .IsRequired();

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

            builder.Entity<Messages>()
                .HasOne(s => s.Recipient)
                .WithMany(l => l.MessagesReceived)
                .HasForeignKey(s => s.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Messages>()
                .HasOne(s => s.Sender)
                .WithMany(l => l.MessagesSent)
                .HasForeignKey(s => s.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
