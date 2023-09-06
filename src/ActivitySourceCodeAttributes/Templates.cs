namespace ActivitySourceCodeAttributes;

public static class Templates
{
    public static string GetActivitySourceClass(
        string? repoOrg, string? repoName,
        string baseFilePath, string commitHash)
    {
        return $@"
#pragma warning disable CS8632
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ActivitySourceCodeAttributes;

public class ActivitySourceWithCodePath
{{
    private readonly System.Diagnostics.ActivitySource _source;

    public string Name => _source.Name;
    public string? Version => _source.Version;

    public ActivitySourceWithCodePath(string name)
    {{
        _source = new System.Diagnostics.ActivitySource(name);
    }}

    public ActivitySourceWithCodePath(string name, string? version = """")
    {{
        _source = new System.Diagnostics.ActivitySource(name, version);
    }}

    public bool HasListeners() => _source.HasListeners();

    public Activity? StartActivity([CallerMemberName] string name = """", ActivityKind kind = ActivityKind.Internal, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = """")
    {{
        var span = _source.StartActivity(name, kind);

        span?.AddCodeAttributes(
           ""{baseFilePath}"", 
           filePath,
           ""https://github.com/{repoOrg}/{repoName}/blob/{commitHash}"",
           lineNumber,
           memberName);
    
        return span;
    }}

    public Activity? StartActivity(string name, ActivityKind kind, ActivityContext parentContext, IEnumerable<KeyValuePair<string, object?>>? tags = null, IEnumerable<ActivityLink>? links = null, DateTimeOffset startTime = default, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = """")
    {{
        var span = _source.StartActivity(name, kind, parentContext, tags, links, startTime);

        span?.AddCodeAttributes(
           ""{baseFilePath}"", 
           filePath,
           ""https://github.com/{repoOrg}/{repoName}/blob/{commitHash}"",
           lineNumber,
           memberName);
    
        return span;
    }}

    public Activity? StartActivity(string name, ActivityKind kind, string parentId, IEnumerable<KeyValuePair<string, object?>>? tags = null, IEnumerable<ActivityLink>? links = null, DateTimeOffset startTime = default, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = """")
    {{
        var span = _source.StartActivity(name, kind, parentId, tags, links, startTime);

        span?.AddCodeAttributes(
           ""{baseFilePath}"", 
           filePath,
           ""https://github.com/{repoOrg}/{repoName}/blob/{commitHash}"",
           lineNumber,
           memberName);
    
        return span;
    }}

    public Activity? StartActivity(ActivityKind kind, ActivityContext parentContext = default, IEnumerable<KeyValuePair<string, object?>>? tags = null, IEnumerable<ActivityLink>? links = null, DateTimeOffset startTime = default, [CallerMemberName] string name = """", [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = """")
    {{
        var span = _source.StartActivity(kind, parentContext, tags, links, startTime, name);

        span?.AddCodeAttributes(
           ""{baseFilePath}"", 
           filePath,
           ""https://github.com/{repoOrg}/{repoName}/blob/{commitHash}"",
           lineNumber,
           name);
    
        return span;
    }}

    public static void AddActivityListener(ActivityListener listener)
    {{
        System.Diagnostics.ActivitySource.AddActivityListener(listener);
    }}

    public Activity? CreateActivity(string name, ActivityKind kind, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = """")
    {{
        var span = _source.CreateActivity(name, kind);

        span?.AddCodeAttributes(
           ""{baseFilePath}"", 
           filePath,
           ""https://github.com/{repoOrg}/{repoName}/blob/{commitHash}"",
           lineNumber,
           memberName);
    
        return span;
    }}

    public Activity? CreateActivity(string name, ActivityKind kind, string parentId, IEnumerable<KeyValuePair<string, object?>>? tags = null, IEnumerable<ActivityLink>? links = null, ActivityIdFormat idFormat = ActivityIdFormat.Unknown, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = """")
    {{
        var span = _source.CreateActivity(name, kind, parentId, tags, links, idFormat);

        span?.AddCodeAttributes(
           ""{baseFilePath}"", 
           filePath,
           ""https://github.com/{repoOrg}/{repoName}/blob/{commitHash}"",
           lineNumber,
           memberName);
    
        return span;
    }}

    public Activity? CreateActivity(string name, ActivityKind kind, ActivityContext parentContext, IEnumerable<KeyValuePair<string, object?>>? tags = null, IEnumerable<ActivityLink>? links = null, ActivityIdFormat idFormat = ActivityIdFormat.Unknown, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = """")
    {{
        var span = _source.CreateActivity(name, kind, parentContext, tags, links, idFormat);

        span?.AddCodeAttributes(
           ""{baseFilePath}"", 
           filePath,
           ""https://github.com/{repoOrg}/{repoName}/blob/{commitHash}"",
           lineNumber,
           memberName);
    
        return span;
    }}

    public void Dispose()
    {{
        _source.Dispose();
    }}
}}
";
    }

    public static string GenerateGlobalUsing()
    {
        return @"
        global using ActivitySource = ActivitySourceCodeAttributes.ActivitySourceWithCodePath;";
    }
}
