using System.Text;
using Innowise.Clinic.Services.Persistence;
using Innowise.Clinic.Services.Persistence.Models;
using Innowise.Clinic.Services.Services.ServiceService.Implementations;
using Innowise.Clinic.Services.Services.ServiceService.Interfaces;
using Innowise.Clinic.Services.Services.SpecializationService.Implementations;
using Innowise.Clinic.Services.Services.SpecializationService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Innowise.Clinic.Services.Configuration;

public static class StartupConfigurator
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<ISpecializationService, SpecializationService>();
        return services;
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                    },
                    new string[] { }
                }
            });
        });
        return services;
    }

    public static IServiceCollection ConfigureSecurity(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    Environment.GetEnvironmentVariable("JWT__KEY") ?? throw new
                        InvalidOperationException()))
            };
        });
        return services;
    }
    
    public static async Task PrepareDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ServicesDbContext>();
            if ((await context.Database.GetPendingMigrationsAsync()).Any()) await context.Database.MigrateAsync();
            await context.Specializations.AddRangeAsync(new List<Specialization>
            {
                new ()
                {
                    Name = "Surgery",
                    IsActive = true,
                    Services = new List<Service>()
                },
                new ()
                {
                    Name = "Cardiology",
                    IsActive = true,
                    Services = new List<Service>()
                },
                new ()
                {
                    Name = "Cancer Care",
                    IsActive = false,
                    Services = new List<Service>()
                }
            });

            await context.SaveChangesAsync();
        }
    }
}