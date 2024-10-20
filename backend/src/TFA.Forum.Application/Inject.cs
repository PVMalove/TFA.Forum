using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Commands.CreateTopic;
using TFA.Forum.Application.Queries.GetAllForums;
using TFA.Forum.Application.Storage.Forum;
using TFA.Forum.Application.Storage.Topic;
using TFA.Forum.Domain;

namespace TFA.Forum.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGetAllForumsUseCase, GetAllForumsUseCase>();
        services.AddScoped<ICreateTopicUseCase, CreateTopicUseCase>();
        
        services.AddScoped<ICreateTopicStorage, CreateTopicStorage>();
        services.AddScoped<IGetAllForumsStorage, GetAllForumsStorage>();
        
        services.AddScoped<IIntentionResolver, TopicIntentionResolver>();
        services.AddScoped<IIntentionManager, IntentionManager>();
        services.AddScoped<IIdentityProvider, IdentityProvider>();
        
        services.AddScoped<IGuidFactory, GuidFactory>();
        services.AddScoped<IMomentProvider, MomentProvider>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }
}