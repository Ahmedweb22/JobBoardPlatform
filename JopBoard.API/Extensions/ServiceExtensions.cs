using JobBoard.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace JopBoard.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfraStructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add your infrastructure services here
            // For example, you can add your DbContext, repositories, etc.
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            return services;
        }
    }
}
