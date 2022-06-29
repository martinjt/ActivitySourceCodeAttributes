using System.Diagnostics;
using System.Reflection;
using Shouldly;

namespace ActivitySourceCodeAttributes.Tests;

public static class ParameterExtensions
{
    public static ParameterInfo[] RemovePatchedCodeParameters(this ParameterInfo[] parameters)
    {
        if (!parameters.Any())
            return parameters;

        if (parameters.Last().Name == "memberName")
            parameters = parameters.Take(parameters.Length - 1).ToArray();

        if (parameters.Last().Name == "lineNumber")
            parameters = parameters.Take(parameters.Length - 1).ToArray();

        if (parameters.Last().Name == "filePath")
            parameters = parameters.Take(parameters.Length - 1).ToArray();

        return parameters;
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