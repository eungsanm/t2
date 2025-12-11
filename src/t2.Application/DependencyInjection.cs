using Microsoft.Extensions.DependencyInjection;
using t2.Application.Interfaces;
using t2.Application.Services;
using t2.Application.Mappings;

namespace t2.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ILoanService, LoanService>();

            return services;
        }
    }
}

