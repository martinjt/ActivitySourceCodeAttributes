using System.Diagnostics;
using Shouldly;

namespace ActivitySourceCodeAttributes.Tests;

public class CodePathTests : ActivitySourceBaseTest
{

    [Fact]
    public void StartActivity_ProducesCodeAttributeWithRightValue()
    {
        AddListenerToTestSource();

        var span = _source.StartActivity("TestActivity");
        span.ShouldNotBeNull();
        span?.Stop();

        var testActivity = triggeredActivities.FirstOrDefault();
        testActivity.ShouldNotBeNull();
        testActivity.ShouldContainTagWithValue("code.filepath", GetFilePath());
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
        testActivity.ShouldContainTagWithValue("code.lineno", (linenumber - 3).ToString());

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
        testActivity.ShouldContainTagWithValue("code.function", nameof(StartActivity_ProducesMethodAttributeWithRightValue));
    }
}

public static class ActivityTestExtensions
{
    public static void ShouldContainTagWithValue(this Activity activity, string name, string value)
    {
        activity.Tags.ShouldContain(t => t.Key == name);
        activity.Tags.ShouldContain(t => t.Key == name && t.Value == value);
    }
}
