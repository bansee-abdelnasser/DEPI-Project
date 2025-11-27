using Eventa.DataAccess.DataContext;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Identity;
using Eventa.DataAccess.Interfaces;
using Eventa.DataAccess.Repositories.Todo.DataAccess.Contracts;
using Eventa.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;


            })
                .AddEntityFrameworkStores<EventaDbContext>();


            services.AddScoped<IUnitOfWork, EventaUnitOfWork>();
            return services;
        }

        public static IServiceCollection AddAuthenticationWithJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection(JwtOptions.sectionName);
            services.Configure<JwtOptions>(jwtSection);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = jwtSection.Get<JwtOptions>();
                var encodedKey = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(encodedKey)
                };
            });

            services.AddScoped<ITokenManager, TokenManager>();
            return services;
        }

        public static async Task SeedRoles(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {

                    var services = scope.ServiceProvider;
                    var dbContext = services.GetRequiredService<EventaDbContext>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    List<string> roles = new List<string>
                {
                    "organizers","users"
                };

                    if (!await dbContext.Database.CanConnectAsync())
                    {
                        await dbContext.Database.MigrateAsync();
                    }

                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                            await roleManager.CreateAsync(new IdentityRole(role));
                    }

                }
            }
        }

    }
}
