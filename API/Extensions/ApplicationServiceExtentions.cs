using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;

using Microsoft.EntityFrameworkCore;
namespace API.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddCors();
            return services;
        }
    }
}