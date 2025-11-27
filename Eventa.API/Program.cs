using Eventa.Application;
using Eventa.DataAccess;
using Eventa.DataAccess.Identity;

namespace Eventa.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


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
