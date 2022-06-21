using System.Diagnostics;

namespace ActivityGenerator;

public static class ActivityExtensions
{
    public static void AddCodeAttributes(this Activity activity, 
        string baseFilePath,
        string? filePath, 
        string repoUrl, 
        int lineNumber, 
        string method)
    {
        if (!string.IsNullOrEmpty(method))
            activity?.SetTag("code.function", method);
        
        if (!string.IsNullOrEmpty(repoUrl) &&
            !string.IsNullOrEmpty(filePath))
        {
            var relativePath = filePath.Replace(baseFilePath, "");
            activity?.SetTag("code.url", GetUrl(repoUrl, relativePath, lineNumber));
            activity?.SetTag("code.filepath", filePath);
        }

        if (lineNumber != 0)
            activity?.SetTag("code.lineno", lineNumber.ToString());

    }

    public static string GetUrl(string repoUrl, string path, int lineNumber)
    {
        return $"{repoUrl}{path}#L{lineNumber}";
    }
}
