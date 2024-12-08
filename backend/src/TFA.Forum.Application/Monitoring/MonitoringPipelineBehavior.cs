using System.Diagnostics;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Application.Monitoring;

internal class MonitoringPipelineBehavior<TRequest, TResponse>(
    ApplicationMetrics metrics,
    ILogger<MonitoringPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResult
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not IMonitoredRequest monitoredRequest) return await next.Invoke();
        var parentContext = Activity.Current?.Context ?? default;
        using var activity = ApplicationMetrics.ActivitySource.StartActivity("usecase", ActivityKind.Internal, parentContext);
        activity?.AddTag("tfa.command", request.GetType().Name);
        
        try
        {
            var result = await next.Invoke();

            if (result is IResult resultObject)
            {
                if (resultObject.IsFailure && resultObject is IError<ErrorList> resultWithError)
                {
                    monitoredRequest.MonitorFailure(metrics);
                    activity?.AddTag("Command error", true);
                    logger.LogWarning("Command handled with failure {Command} - {Error}", request, resultWithError.Error);
                }
                else
                {
                    monitoredRequest.MonitorSuccess(metrics);
                    activity?.AddTag("Command error", false);
                    logger.LogInformation("Command successfully handled {Command}", request);
                }
            }
            
            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled error caught while handling command {Command}", request);
            monitoredRequest.MonitorFailure(metrics);
            activity?.AddTag("Exception error", true);
            throw;
        }
    }
}