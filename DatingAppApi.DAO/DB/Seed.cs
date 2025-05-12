using DatingAppApi.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DatingAppApi.DAL.DB
{
    public class Seed
    {
        //public static async Task SeedUsers(AppDBContext context)
        public static async Task SeedUsers(UserManager<AppUsers> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync())
                return;

            var userData = await File.ReadAllTextAsync("DB/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUsers>>(userData, options);

            if (users == null)
                return;

            var roles = new List<AppRole>
            {
                new AppRole() { Name = "Admin" },
                new AppRole() { Name = "Moderator" },
                new AppRole() { Name = "Member" },
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUsers
            {
                UserName = "admin",
                KnownAs = "Admin",
                Gender = "",
                City = "",
                Country = ""
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);

            foreach (var user in users)
            {
                //using var hmac = new HMACSHA512();

                user.UserName = user.UserName!.ToLower();
                //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                //user.PasswordSalt = hmac.Key;

                //userManager.Users.Add(user);

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
            }

            //var rowsAffected = await userManager.SaveChangesAsync();
            //Console.WriteLine($"Rows affected: {rowsAffected}");
        }
    }
}
