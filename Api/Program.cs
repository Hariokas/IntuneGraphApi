using Api.Middleware;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Implementations;
using Repositories.Interfaces;
using Serilog;
using Services.Implementations;
using Services.Interfaces;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Starting up the application");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            Log.Information("Configuring services");

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure Graph API settings
            Log.Information("Configuring Graph API settings");
            builder.Services.Configure<GraphApiConfiguration>(builder.Configuration.GetSection("GraphApi"));

            // Add cache service
            Log.Information("Adding cache service");
            builder.Services.AddControllersWithViews(options =>
            {
                options.CacheProfiles.Add("5MinCache", new CacheProfile()
                {
                    Duration = 300,
                    Location = ResponseCacheLocation.Any
                });
            });

            // Add dependencies
            Log.Information("Registering dependencies");
            builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
            builder.Services.AddScoped<IGraphClientFactory, GraphClientFactory>();

            builder.Services.AddScoped<IAppRepository, AppRepository>();
            builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();

            builder.Services.AddScoped<IAppService, AppService>();
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<IGroupService, GroupService>();

            var app = builder.Build();

            Log.Information("Adding global exception handler middleware");
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                Log.Information("In Development environment - enabling Swagger");
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            //app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            Log.Information("Starting the application");
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application start-up failed");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}