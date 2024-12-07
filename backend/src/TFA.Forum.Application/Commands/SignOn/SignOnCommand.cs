using CSharpFunctionalExtensions;
using MediatR;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Monitoring;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Application.Commands.SignOn;

public record SignOnCommand(string Login, string Password) : IRequest<Result<IIdentity, ErrorList>>, IMonitoredRequest
{
    private const string CounterName = "account.signedon";
    
    public void MonitorSuccess(ApplicationMetrics metrics) => 
        metrics.IncrementCount(CounterName, 1, ApplicationMetrics.ResultTags(true));

    public void MonitorFailure(ApplicationMetrics metrics) => 
        metrics.IncrementCount(CounterName, 1, ApplicationMetrics.ResultTags(false));
}