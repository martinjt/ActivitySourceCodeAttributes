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

}