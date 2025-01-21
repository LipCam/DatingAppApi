using DatingAppApi.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DatingAppApi.DAL.DB
{
    public class Seed
    {
        public static async Task SeedUsers(AppDBContext context)
        {
            if (await context.AppUsers.AnyAsync())
                return;

            var userData = await File.ReadAllTextAsync("DB/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUsers>>(userData, options);

            if (users == null)
                return;

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName!.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                context.AppUsers.Add(user);
            }

            var rowsAffected = await context.SaveChangesAsync();
            Console.WriteLine($"Rows affected: {rowsAffected}");
        }
    }
}
