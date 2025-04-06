
using EmployeeManagement.Contexts;
using EmployeeManagement.Core.Settings;
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
                    "Server=localhost;Port=3306;Database=employee_management;User=root;Password=Codeinsight@123",
                    new MySqlServerVersion(new Version(8, 0, 34)))
                    );

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
