namespace TFA.Forum.Application.Monitoring;

internal interface IMonitoredRequest
{
    void MonitorSuccess(ApplicationMetrics metrics);
    void MonitorFailure(ApplicationMetrics metrics);
}