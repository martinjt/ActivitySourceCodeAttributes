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

    [Fact]
    public void StartActivity_ProducesCodeUrlAttributeWithRightValue()
    {
        AddListenerToTestSource();

        var span = _source.StartActivity("TestActivity");
        span.ShouldNotBeNull();
        span?.Stop();

        var testActivity = triggeredActivities.FirstOrDefault();
        testActivity.ShouldNotBeNull();
        testActivity.Tags.ShouldContain(t => t.Key == "code.url");
        var codeurl = testActivity.Tags.First(t => t.Key == "code.url").Value;

        var thisFile = new FileInfo(GetFilePath());
        codeurl.ShouldStartWith("https://github.com/martinjt");
        var codeUrlWithoutLine = codeurl.Split('#')[0];
        codeUrlWithoutLine.ShouldEndWith(thisFile.Name);
    }
}
