using Eventa.DataAccess.DataContext;
using Eventa.DataAccess.Interfaces;
using Eventa.DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
 

            services.AddDbContext<EventaDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("EventaDb");
                options.UseSqlServer(connectionString);
            });
            services.AddScoped<IUnitOfWork, EventaUnitOfWork>();
            return services;
        }
    }
}
