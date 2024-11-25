using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Domain.Interfaces.Repository;
using TFA.Forum.Persistence.Repositories;
using TFA.Forum.Persistence.Storage.Forum;
using TFA.Forum.Persistence.Storage.Topic;
using TFA.Forum.Persistence.Storage.User;

namespace TFA.Forum.Persistence;

public static class Inject
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");
        services.AddDbContextPool<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
        
        services.AddScoped<IBaseRepository<Domain.Entities.Forum>, BaseRepository<Domain.Entities.Forum>>();
        services.AddScoped<IBaseRepository<Topic>, BaseRepository<Topic>>();
        services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();

        services.AddScoped<IGetAllForumsStorage, GetAllForumsStorage>();
        services.AddScoped<ICreateForumStorage, CreateForumStorage>();
        services.AddScoped<IGetTopicsStorage, GetTopicsStorage>();
        services.AddScoped<ICreateTopicStorage, CreateTopicStorage>();
        services.AddScoped<ISignInStorage, SignInStorage>();
        services.AddScoped<ISignOnStorage, SignOnStorage>();
        
        return services;
    }
}