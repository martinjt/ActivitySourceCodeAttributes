using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ActivitySourceCodeAttributes.Tests;

public class ActivitySourceBaseTest
{
    private const string SourceName = "TestSource";
    internal readonly ActivitySource _source = new ActivitySource(SourceName);
    internal readonly ActivityListener _listener = new ActivityListener
    {
        SampleUsingParentId = (ref ActivityCreationOptions<string> activityOptions) => ActivitySamplingResult.AllData,
        Sample = (ref ActivityCreationOptions<ActivityContext> activityOptions) => ActivitySamplingResult.AllData
    };
    internal readonly List<Activity> triggeredActivities = new List<Activity>();


    internal void AddListenerToTestSource()
    {
        _listener.ActivityStopped += activity => triggeredActivities.Add(activity);
        _listener.ShouldListenTo = (activitySource) => activitySource.Name == SourceName;

        System.Diagnostics.ActivitySource.AddActivityListener(_listener);
    }

    internal string GetFilePath([CallerFilePath] string filePath = null!)
    {
        return filePath;
    }

    internal int GetLineNumber([CallerLineNumber] int lineNumber = 0)
    {
        return lineNumber;
    }
}
