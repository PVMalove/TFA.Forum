﻿using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;
using TFA.Forum.API;
using TFA.Forum.Persistence;

namespace TFA.Forum.E2E;

public class ForumApiApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer dbContainer = new PostgreSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DbConnection"] = dbContainer.GetConnectionString(),
                ["Authentication:Base64Key"] = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                ["Serilog:WriteTo:0:Name"] = "Console",
            }!)
            .Build();
        builder.UseConfiguration(configuration);
        builder.ConfigureLogging(cfg => cfg.ClearProviders());
        base.ConfigureWebHost(builder);
    }

    public async Task InitializeAsync()
    {
        await dbContainer.StartAsync();
        var forumDbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(dbContainer.GetConnectionString()).Options);
        await forumDbContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync() => await dbContainer.DisposeAsync();
}