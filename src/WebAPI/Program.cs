
using Application;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;
using WebAPI.Middleware;

namespace WebAPI;

public class Program
{
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
        builder.Services.AddSwaggerGen();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(dbConnectionString);

        //change logger
        builder.Logging.ClearProviders();
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Logging.AddSerilog(logger);

        builder.Services.AddHttpClient();

        var app = builder.Build();

        //custom middleware
        app.UseMiddleware<ErrorChecking>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1"));

        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
