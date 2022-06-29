using System.Diagnostics;
using System.Reflection;
using Shouldly;

namespace ActivitySourceCodeAttributes.Tests;

public class NewActivitySource_HasTheSameStructureAsBCL
{
    [Fact]
    public void InternalActivitySource_HasAllMethodsFromBCLClassWithCorrectCount()
    {
        var activitySource = new ActivitySource("test");
        var bclActivitySource = new System.Diagnostics.ActivitySource("bclSource");

        var bclMethodNames = bclActivitySource
            .GetType()
            .GetMethods()
            .GroupBy(m => m.Name);

        var newMethodNames = activitySource
            .GetType()
            .GetMethods()
            .GroupBy(m => m.Name);

        foreach (var method in bclMethodNames)
        {
            newMethodNames.ShouldContain(m => m.Key == method.Key);

            newMethodNames.First(m => m.Key == method.Key)
                .Count()
                .ShouldBe(method.Count(), $"Wrong Method name count for {method.Key}");
        }
    }

    [Fact]
    public void InternalActivitySource_MethodsFromNewClassMatchParamsOtherThanCodeAttributes()
    {
        var activitySource = new ActivitySource("test");
        var bclActivitySource = new System.Diagnostics.ActivitySource("bclSource");

        var bclMethods = bclActivitySource
            .GetType()
            .GetMethods()
            .Where(m => !m.Name.StartsWith("get_"))
            .ToList();

        var newMethods = activitySource
            .GetType()
            .GetMethods()
            .Where(m => !m.Name.StartsWith("get_"))
            .ToList();

        foreach (var originalMethod in bclMethods)
        {
            var newMethodFound = false;
            var originalParameters = originalMethod
                .GetParameters()
                .ToList();

            var newMethodsWithSameName = newMethods
                .Where(m => m.Name == originalMethod.Name)
                .ToList();

            foreach (var possibleNewMethod in newMethodsWithSameName)
            {
                var newMethodParameters = possibleNewMethod
                    .GetParameters()
                    .RemovePatchedCodeParameters()
                    .ToList();

                if (newMethodParameters.Count != originalParameters.Count)
                    continue;

                var newMethodParameterMatch = true;
                for (int i = 0; i < originalParameters.Count; i++)
                {
                    if (!ParametersMatch(originalParameters[i], newMethodParameters[i]))
                    {
                        newMethodParameterMatch = false;
                        break;
                    }
                }

                if (newMethodParameterMatch)
                {
                    newMethodFound = true;
                    break;
                }
            }

            Assert.True(newMethodFound, $"No method found for {originalMethod.Name}({string.Join(',', originalMethod.GetParameters().Select(p => p.ParameterType.Name.ToString()))})");
                
        }

    }

    public static bool ParametersMatch(ParameterInfo originalParameter, ParameterInfo newParameter)
    {
        return originalParameter.Name == newParameter.Name &&
               originalParameter.ParameterType == newParameter.ParameterType;
    }
}
