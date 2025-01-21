using DatingAppApi.API.Helpers;
using DatingAppApi.API.Services;
using DatingAppApi.API.Services.Interfaces;
using DatingAppApi.BLL.Services;
using DatingAppApi.BLL.Services.Interfaces;
using DatingAppApi.DAL.DB;
using DatingAppApi.DAL.Repositories;
using DatingAppApi.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<AppDBContext>(p => p.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            return services;
        }
    }
}
