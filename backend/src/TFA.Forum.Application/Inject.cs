using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Commands.CreateForum;
using TFA.Forum.Application.Commands.CreateTopic;
using TFA.Forum.Application.Commands.SingOut;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Application.Mapping;
using TFA.Forum.Application.Monitoring;
using TFA.Forum.Domain.EntityIds;
using TFA.Forum.Domain.Interfaces;

namespace TFA.Forum.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ForumMapping));

        services.AddMediatR(c => c
            .RegisterServicesFromAssembly(typeof(Inject).Assembly)
            .AddOpenBehavior(typeof(MonitoringPipelineBehavior<,>))
            .AddOpenBehavior(typeof(ResultValidationPipelineBehavior<,>)));
        
        services.AddScoped<IIntentionResolver, ForumIntentionResolver>();
        services.AddScoped<IIntentionResolver, TopicIntentionResolver>();
        services.AddScoped<IIntentionResolver, AccountIntentionResolver>();
        
        services.AddScoped<IIntentionManager, IntentionManager>();
        services.AddScoped<IIdentityProvider, IdentityProvider>();
        services.AddScoped<IPasswordManager, PasswordManager>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ISymmetricDecryptor, AesSymmetric>();
        services.AddScoped<ISymmetricEncryptor, AesSymmetric>();

        services.AddScoped<IMomentProvider, MomentProvider>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        services.AddMemoryCache();
        services.AddHandlers();
        
        services.AddSingleton<ApplicationMetrics>();
        
        return services;
    }
    
    private static IServiceCollection AddHandlers(this IServiceCollection collection)
    {
        collection.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        collection.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        return collection;
    }
}