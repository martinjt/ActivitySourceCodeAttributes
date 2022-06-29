using System.Diagnostics;
using Shouldly;

namespace ActivitySourceCodeAttributes.Tests;

public class GlobalUsingTests
{
    [Fact]
    public void InternalActivitySource_HasHadTypeReplaced()
    {
        var activitySource = new ActivitySource("test");
        activitySource.GetType().Name.ShouldBe("ActivitySourceWithCodePath");
    }

    [Fact]
    public void InternalActivitySource_DoesNotOverrideFullyQuantifiedClass()
    {
        var bclActivitySource = new System.Diagnostics.ActivitySource("bclSource");
        bclActivitySource.GetType().Name.ShouldBe("ActivitySource");
    }    
}
