using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using VendingMachine.API.Extensions;
using VendingMachine.Core.Interfaces.Auth;
using VendingMachine.Core.Interfaces.Mappers;
using VendingMachine.Core.Interfaces.Repositories;
using VendingMachine.Core.Models;
using VendingMachine.Core.Services;
using VendingMachine.DataAccess;
using VendingMachine.DataAccess.Entites;
using VendingMachine.DataAccess.Mappers;
using VendingMachine.DataAccess.Repositories;
using VendingMachine.Infrastructure;

namespace VendingMachine.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(
               new WebApplicationOptions { WebRootPath = "Static/Img" });
            var services = builder.Services;
            var configuration = builder.Configuration;

            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddDbContext<VendingMachineDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString(nameof(VendingMachineDbContext))));

            services.AddCors();

            services.AddApiAuthentication(configuration);

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IDrinksRepository, DrinksRepository>();
            services.AddScoped<IVendingMachinesRepository, VendingMachinesRepository>();

            services.AddScoped<IMapper<User, UserEntity>, UsersMapper>();
            services.AddScoped<IMapper<Drink, DrinkEntity>, DrinksMapper>();
            services.AddScoped<IMapper<Core.Models.VendingMachine, VendingMachineEntity>, VendingMachinesMapper>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();

            services.AddScoped<UsersService>();
            services.AddScoped<VendingMachinesService>();
            services.AddScoped<DrinksService>();

            services.AddSingleton(s => new ImageService(builder.Environment.WebRootPath));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<Middlewares.ExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors(c => c
                .WithOrigins(configuration["UrlClient"]!)
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.MapControllers();

            app.Run();
        }
    }
}
