﻿using Asp.Versioning;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using TFA.Forum.API.Authentication;
using TFA.Forum.API.Configurations;
using TFA.Forum.API.Monitoring;
using TFA.Forum.Application;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Persistence;

namespace TFA.Forum.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddControllers();

        services.AddPersistence(builder.Configuration);
        services.AddApplication();
        services.AddScoped<IAuthTokenStorage, AuthTokenStorage>();
        services.Configure<AuthenticationConfiguration>(builder.Configuration.GetSection("Authentication").Bind);
        services.AddApiLogging(builder.Configuration, builder.Environment);
        services.AddApiMetrics(builder.Configuration);
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        
         services.AddApiVersioning()
             .AddApiExplorer(options =>
             {
                 options.DefaultApiVersion = new ApiVersion(1.0);
                 options.GroupNameFormat = "'v'VVV";
                 options.SubstituteApiVersionInUrl = true;
                 options.AssumeDefaultVersionWhenUnspecified = true;
             });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}