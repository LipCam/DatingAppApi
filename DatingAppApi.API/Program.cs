using DatingAppApi.API.Extensions;
using DatingAppApi.API.Middleware;
using DatingAppApi.DAL.DB;
using DatingAppApi.DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DatingAppApi.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationService(builder.Configuration);

            builder.Services.AddIdentityService(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200") // Permita apenas essa origem específica
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            app.UseCors("AllowSpecificOrigin");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
                        
            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            try
            {
                //com AspNetCore.Identity
                var context = service.GetRequiredService<AppDBContext>();
                var userManager = service.GetRequiredService<UserManager<AppUsers>>();
                var roleManager = service.GetRequiredService<RoleManager<AppRole>>();

                //Executando as migrations que não foram executadas e/ou criando o banco
                await context.Database.MigrateAsync();

                //Executando a inserção de dados no banco caso o banco esteja vazio
                await Seed.SeedUsers(userManager, roleManager);


                //sem AspNetCore.Identity
                //var context = service.GetRequiredService<AppDBContext>();
                ////Executando as migrations que não foram executadas e/ou criando o banco
                //await context.Database.MigrateAsync();

                ////Executando a inserção de dados no banco caso o banco esteja vazio
                //await Seed.SeedUsers(context);
            }
            catch (Exception ex)
            {
                var logger = service.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during migration");
            }

            app.Run();
        }
    }
}
