
using Application;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;
using WebAPI.Middleware;

namespace WebAPI;
/// <summary>
/// Main entry point for the application.
/// </summary>
public class Program
{
    /// <summary>
    /// Main method that starts the application.
    /// </summary>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string? dbConnectionString = builder.Configuration.GetConnectionString("PostgreConnection");
        // Add services to the container.

        builder.Services.AddControllers()
                        .AddJsonOptions(options =>
                        {
                            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(dbConnectionString);
        builder.Services.AddJWTAuthenticate(builder.Configuration);

        //change logger
        builder.Logging.ClearProviders();
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Logging.AddSerilog(logger);

        builder.Services.AddHttpClient();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CacheTask", Version = "v1" });

            // Include the XML comments file
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        var app = builder.Build();

        //custom middleware
        app.UseMiddleware<ErrorChecking>();

        //Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1"));
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}
