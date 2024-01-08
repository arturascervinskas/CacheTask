using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.Text;
using WebAPI.Services;

namespace WebAPI;

/// <summary>
/// Provides methods to configure JWT authentication in the service collection.
/// </summary>
public static class JWTAuthenticateInjection
{
    /// <summary>
    /// Adds JWT authentication configuration to the provided service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="config">The configuration object containing JWT settings.</param>
    public static void AddJWTAuthenticate(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IJWTService, JWTService>();

        string secretKey = config["Jwt:SecretKey"] ?? throw new InvalidDataException("JWT SecretKey");
        string issuer = config["Jwt:Issuer"] ?? throw new InvalidDataException("JWT Issuer");
        string audience = config["Jwt:Audience"] ?? throw new InvalidDataException("JWT audience");

        var swaggerGenServiceDescriptor = services.SingleOrDefault
        (
            d => d.ServiceType == typeof(ISwaggerProvider)
        );

        if (swaggerGenServiceDescriptor != null)
        {
            services.Remove(swaggerGenServiceDescriptor);
        }

        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "CacheTask", Version = "v1" });

            // Include the XML comments file
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            option.IncludeXmlComments(xmlPath);

            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
            });
        });

        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });
    }
}
