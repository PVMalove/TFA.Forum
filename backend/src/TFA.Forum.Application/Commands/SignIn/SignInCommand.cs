using CSharpFunctionalExtensions;
using MediatR;
using TFA.Forum.Application.Monitoring;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Application.Commands.SignIn;

public record SignInCommand(string Login, string Password, bool RememberMe)
    : IRequest<Result<SignInResponse, ErrorList>>, IMonitoredRequest
{
    private const string CounterName = "account.signedin";
    
    public void MonitorSuccess(ApplicationMetrics metrics) => 
        metrics.IncrementCount(CounterName, 1, ApplicationMetrics.ResultTags(true));

    public void MonitorFailure(ApplicationMetrics metrics) => 
        metrics.IncrementCount(CounterName, 1, ApplicationMetrics.ResultTags(false));
}