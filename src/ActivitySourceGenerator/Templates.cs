namespace ActivitySourceGenerator;

public static class Templates
{
    public static string GetActivitySourceClass(
        string? repoOrg, string? repoName,
        string baseFilePath, string commitHash)
    {
        return $@"
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ActivityGenerator;

public class ActivitySourceWithCodePath
{{
    private readonly System.Diagnostics.ActivitySource _source;
    public ActivitySourceWithCodePath(string name)
    {{
        _source = new System.Diagnostics.ActivitySource(name);
    }}

    public Activity? StartActivity(string name = """", [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = """")
    {{
        var span = _source.StartActivity(name);

        span?.AddCodeAttributes(
           ""{baseFilePath}"", 
           filePath,
           ""https://github.com/{repoOrg}/{repoName}/blob/{commitHash}"",
           lineNumber,
           memberName);
    
        return span;
    }}

}}
";
    }

    public static string GenerateGlobalUsing()
    {
        return @"
        global using ActivitySource = ActivityGenerator.ActivitySourceWithCodePath;";
    }
}
