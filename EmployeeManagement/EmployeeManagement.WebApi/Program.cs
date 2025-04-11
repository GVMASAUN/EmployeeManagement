
using EmployeeManagement.Contexts;
using EmployeeManagement.Core.Contracts;
using EmployeeManagement.Core.Services;
using EmployeeManagement.Core.Settings;
using EmployeeManagement.DataAccess;
using EmployeeManagement.DataAccess.Contracts;
using EmployeeManagement.Services.Contracts;
using EmployeeManagement.Services.Features;
using EmployeeManagement.Services.Services;
using EmployeeManagement.WebApi.Authentication;
using EmployeeManagement.WebApi.Authorization.Handlers;
using EmployeeManagement.WebApi.Authorization.Requirements;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

            builder.Services.AddAuthentication("JwtScheme")
                .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("JwtScheme", null);

            builder.Services.AddLogging();
            builder.Services.AddAuthentication("JwtScheme");
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                    policy.Requirements.Add(new RoleRequirement("Admin")));
                options.AddPolicy("EmployeePolicy", policy =>
                    policy.Requirements.Add(new RoleRequirement("User")));
            });

            builder.Services.AddSingleton<IAuthorizationHandler, RoleRequirementHandler>();
            //builder.Services.AddAuthorization();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetSection("AppSettings")["Redis"];
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHostedService<QueuedProcessorBackgroundService>();
            builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
            builder.Services.AddScoped<IMapper, Mapper>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IClaimsPrincipalProvider, ClaimsPrincipalProvider>();
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
