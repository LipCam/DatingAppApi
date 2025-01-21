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
    }
}
