using Microsoft.EntityFrameworkCore;
using TFA.Forum.Persistence;

namespace TFA.Forum.API.Extensions;

public static class AppExtensions
{
    public static async Task DbInitializer(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        try
        {
            await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();
            Console.WriteLine($"Initializing database: {dbContext.Database.ProviderName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing database: {ex.Message}");
            throw;
        }
    }
}