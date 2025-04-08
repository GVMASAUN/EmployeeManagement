
using EmployeeManagement.Contexts;
using EmployeeManagement.Core.Settings;
using EmployeeManagement.DataAccess;
using EmployeeManagement.DataAccess.Contracts;
using EmployeeManagement.Services.Features;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var appSetting = new AppSetting();
            builder.Configuration.GetSection("AppSetting").Bind(appSetting);
            builder.Services.AddSingleton(appSetting);


            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetSection("AppSettings")["ConnectionString"],
                    new MySqlServerVersion(new Version(8, 0, 34)))
                    );

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetSection("AppSettings")["Redis"];
            });

            builder.Services.AddScoped<IMapper, Mapper>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetAllEmployees).Assembly));


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
