using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Commands.CreateForum;
using TFA.Forum.Application.Commands.CreateTopic;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Application.Mapping;
using TFA.Forum.Application.Queries.GetAllForums;
using TFA.Forum.Application.Queries.GetTopics;
using TFA.Forum.Domain.Interfaces;
using TFA.Forum.Persistence.Shared;

namespace TFA.Forum.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ForumMapping));
        
        services.AddScoped<IGetAllForumsUseCase, GetAllForumsUseCase>();
        services.AddScoped<ICreateTopicUseCase, CreateTopicUseCase>();
        services.AddScoped<IGetTopicsUseCase, GetTopicsUseCase>();

        services.AddScoped<IIntentionResolver, ForumIntentionResolver>();
        services.AddScoped<IIntentionResolver, TopicIntentionResolver>();
        
        services.AddScoped<IIntentionManager, IntentionManager>();
        services.AddScoped<IIdentityProvider, IdentityProvider>();
        
        services.AddScoped<IGuidFactory, GuidFactory>();
        services.AddScoped<IMomentProvider, MomentProvider>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        services.AddMemoryCache();
        services.AddHandlers();
        
        return services;
    }
    
    private static IServiceCollection AddHandlers(this IServiceCollection collection)
    {
        collection.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return collection;
    }
}