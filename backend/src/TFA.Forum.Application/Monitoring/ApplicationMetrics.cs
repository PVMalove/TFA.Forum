using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace TFA.Forum.Application.Monitoring;

public class ApplicationMetrics(IMeterFactory meterFactory)
{
    private const string ApplicationName = "TFA.Forum.Application";

    private readonly Meter meter = meterFactory.Create(ApplicationName);
    private readonly ConcurrentDictionary<string, Counter<int>> counters = new();

    public static readonly ActivitySource ActivitySource = new(ApplicationName);

    public void IncrementCount(string name, int value, IDictionary<string, object?>? additionalTags = null)
    {
        var counter = counters.GetOrAdd(name, _ => meter.CreateCounter<int>(name));
        counter.Add(value, additionalTags?.ToArray() ?? ReadOnlySpan<KeyValuePair<string, object?>>.Empty);
    }

    public static IDictionary<string, object?> ResultTags(bool success) => new Dictionary<string, object?>
    {
        ["success"] = success
    };
}