using System.Diagnostics;
using System.Runtime.CompilerServices;
using Shouldly;

namespace ActivitySourceCodeAttributes.Tests;

public class CodePathTests
{
    private const string SourceName = "TestSource";
    private readonly ActivitySource _source = new ActivitySource(SourceName);
    private readonly ActivityListener _listener = new ActivityListener
    {
        SampleUsingParentId = (ref ActivityCreationOptions<string> activityOptions) => ActivitySamplingResult.AllData,
        Sample = (ref ActivityCreationOptions<ActivityContext> activityOptions) => ActivitySamplingResult.AllData
    };
    private readonly List<Activity> triggeredActivities = new List<Activity>();

    [Fact]
    public void InternalActivitySource_HasHadTypeReplaced()
    {
        _source.GetType().Name.ShouldBe("ActivitySourceWithCodePath");
    }

    [Fact]
    public void StartActivity_ProducesMethodAttributeWithRightValue()
    {
        AddListenerToTestSource();

        var span = _source.StartActivity("TestActivity");
        span.ShouldNotBeNull();
        span?.Stop();

        var testActivity = triggeredActivities.FirstOrDefault();
        testActivity.ShouldNotBeNull();

        testActivity.Tags.ShouldContain(t => t.Key == "code.function");
        testActivity.Tags.ShouldContain(t => t.Key == "code.function" && t.Value == nameof(StartActivity_ProducesMethodAttributeWithRightValue));
    }

    [Fact]
    public void StartActivity_ProducesCodeAttributeWithRightValue()
    {
        AddListenerToTestSource();

        var span = _source.StartActivity("TestActivity");
        span.ShouldNotBeNull();
        span?.Stop();

        var testActivity = triggeredActivities.FirstOrDefault();
        testActivity.ShouldNotBeNull();


        testActivity.Tags.ShouldContain(t => t.Key == "code.filepath");
        var filePath = GetFilePath();

        testActivity.Tags.ShouldContain(t => t.Key == "code.filepath" && t.Value == filePath);
    }

    [Fact]
    public void StartActivity_ProducesLineNumberAttributeWithRightValue()
    {
        AddListenerToTestSource();

        var span = _source.StartActivity("TestActivity");
        span.ShouldNotBeNull();
        span?.Stop();
        var linenumber = GetLineNumber();

        var testActivity = triggeredActivities.FirstOrDefault();
        testActivity.ShouldNotBeNull();

        testActivity.Tags.ShouldContain(t => t.Key == "code.lineno");
        testActivity.Tags.ShouldContain(t => t.Key == "code.lineno" && t.Value == (linenumber - 3).ToString());
    }

    private void AddListenerToTestSource()
    {
        _listener.ActivityStopped += activity => triggeredActivities.Add(activity);
        _listener.ShouldListenTo = (activitySource) => activitySource.Name == SourceName;

        System.Diagnostics.ActivitySource.AddActivityListener(_listener);
    }

    private int GetLineNumber([CallerLineNumber] int lineNumber = 0)
    {
        return lineNumber;
    }

    private string GetFilePath([CallerFilePath] string filePath = null!)
    {
        return filePath;
    }
}