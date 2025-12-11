using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using t2.Domain.Ports.Out;
using t2.Infrastructure.Persistence.Context;
using t2.Infrastructure.Persistence.Repositories;

namespace t2.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString;
            
            var host = Environment.GetEnvironmentVariable("MYSQL_SERVER");
            var port = Environment.GetEnvironmentVariable("MYSQL_PORT");
            var database = Environment.GetEnvironmentVariable("MYSQL_DATABASE");
            var user = Environment.GetEnvironmentVariable("MYSQL_USER");
            var password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");

            if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(database))
            {
                connectionString = $"Server={host};Port={port ?? "3306"};Database={database};User={user ?? "root"};Password={password ?? ""}";
            }
            else
            {
                connectionString = configuration.GetConnectionString("DefaultConnection") 
                    ?? throw new InvalidOperationException("Variables de entorno mal configuradas.");
            }

            Console.WriteLine($"Connection String: {connectionString}");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(8, 0, 0))
                )
            );

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IBookBajaRepository, BookBajaRepository>();
            services.AddScoped<IBookLiquidacionRepository, BookLiquidacionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}

