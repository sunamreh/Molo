using Microsoft.EntityFrameworkCore;
using Molo.Api.Filters;
using Molo.Application;
using Molo.Infrastructure;
using Molo.Infrastructure.Database;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOptions();

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            // Add services to the container.

            builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>());

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            
            using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<MoloDbContext>();
                context.Database.Migrate();
            }

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}