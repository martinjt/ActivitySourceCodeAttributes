namespace ActivitySourceGenerator;

public static class Templates
{
    public static string GetActivitySourceClass(
        string repoOrg, string repoName,
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

    public Activity? StartActivity(string name = """", [CallerFilePath] string path = null, [CallerLineNumber] int? lineNumber = null)
    {{
        var span = _source.StartActivity(name);
        span?.SetTag(""code.url"", GetPath(path, lineNumber ?? 0));
        span?.SetTag(""code.path"", path);
        span?.SetTag(""code.linenumber"", lineNumber);
        Console.WriteLine($""here: {{path}}"");
        return span;
    }}

    public string GetPath(string path, int lineNumber)
    {{
        return $""https://github.com/{repoOrg}/{repoName}/blob/{commitHash}/{{path}}#L{{lineNumber}}"";
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
