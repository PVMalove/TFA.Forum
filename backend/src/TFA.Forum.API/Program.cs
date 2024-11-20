using Asp.Versioning.ApiExplorer;
using Serilog;
using Serilog.Filters;
using TFA.Forum.API.Extensions;
using TFA.Forum.API.Middlewares;

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

app.UseCors(x=> x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.Run();

namespace TFA.Forum.API
{
    public partial class Program { }
}
