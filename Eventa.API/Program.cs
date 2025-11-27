using Eventa.Application;
using Eventa.DataAccess;
using Eventa.DataAccess.Identity;
using Eventa.Application.Interfaces;
using Eventa.Application.Services;
using Eventa.DataAccess.Repositories;
using Eventa.DataAccess.Interfaces;
using Eventa.DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Eventa.DataAccess.UnitOfWork;
using AutoMapper;
using Eventa.Application.MappingProfiles;

namespace Eventa.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // DbContext
            builder.Services.AddDbContext<EventaDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("EventaDb")));

            // Repositories
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IEventRepository, EventRepository>();

            // Unit of Work
            builder.Services.AddScoped<IUnitOfWork, EventaUnitOfWork>();

            // Services
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IEventService, EventService>();

            builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
            builder.Services.AddAutoMapper(
            typeof(CategoryProfile),
            typeof(EventProfile),
            typeof(AnnouncementProfile)
             );
            builder.Services.AddAutoMapper(typeof(CategoryProfile));





            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddCustomSwaggerGen();


            var JwtSection = builder.Configuration.GetSection(JwtOptions.sectionName);

            builder.Services.Configure<JwtOptions>(JwtSection);

            builder.Services
                 .AddDataAccessServices(builder.Configuration) // minimal: DbContext + Identity
                 .AddApplicationServices(); // your application layer services

            builder.Services.AddAuthenticationWithJWT(builder.Configuration);

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                _ = app.SeedRoles();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
