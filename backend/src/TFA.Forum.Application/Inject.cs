using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Commands.CreateForum;
using TFA.Forum.Application.Commands.CreateTopic;
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
        services.AddScoped<IGetAllForumsUseCase, GetAllForumsUseCase>();
        services.AddScoped<ICreateForumUseCase, CreateForumUseCase>();
        services.AddScoped<ICreateTopicUseCase, CreateTopicUseCase>();
        services.AddScoped<IGetTopicsUseCase, GetTopicsUseCase>();

        services.AddScoped<IIntentionResolver, ForumIntentionResolver>();
        services.AddScoped<IIntentionResolver, TopicIntentionResolver>();
        
        services.AddScoped<IIntentionManager, IntentionManager>();
        services.AddScoped<IIdentityProvider, IdentityProvider>();
        
        services.AddScoped<IGuidFactory, GuidFactory>();
        services.AddScoped<IMomentProvider, MomentProvider>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        services.AddAutoMapper(typeof(ForumMapping));
        
        services.AddMemoryCache();
        
        return services;
    }
}