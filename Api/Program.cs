using Helpers;
using Microsoft.AspNetCore.Mvc;
using Repositories.Implementations;
using Repositories.Interfaces;
using Services.Implementations;
using Services.Interfaces;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configure Graph API settings
        builder.Services.Configure<GraphApiConfiguration>(builder.Configuration.GetSection("GraphApi"));

        // Add cache service
        builder.Services.AddControllersWithViews(options =>
        {
            options.CacheProfiles.Add("5MinCache", new CacheProfile()
            {
                Duration = 300,
                Location = ResponseCacheLocation.Any
            });
        });

        // Add dependencies
        builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
        builder.Services.AddScoped<IGraphClientFactory, GraphClientFactory>();

        builder.Services.AddScoped<IAppRepository, AppRepository>();
        builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
        builder.Services.AddScoped<IGroupRepository, GroupRepository>();

        builder.Services.AddScoped<IAppService, AppService>();
        builder.Services.AddScoped<IDeviceService, DeviceService>();
        builder.Services.AddScoped<IGroupService, GroupService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
        //app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}