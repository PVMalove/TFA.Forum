using Asp.Versioning.ApiExplorer;
using TFA.Forum.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

IServiceCollection services = builder.Services;
services.AddCustomServices(builder);

var app = builder.Build();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

await app.DbInitializer();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
    {
        config.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
            $"Forum swagger {description.GroupName.ToUpperInvariant()}");
    }
});

app.MapControllers();
app.Run();
