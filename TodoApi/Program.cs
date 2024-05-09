using clipboard_project.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace clipboard_project.Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            IServiceCollection serviceCollection = builder.Services.AddDbContext<CloudDBContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();


            app.MapControllers();
            app.Run();
        }

    }
}
