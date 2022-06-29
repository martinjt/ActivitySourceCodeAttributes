using System.Reflection;
using Microsoft.CodeAnalysis;

namespace ActivitySourceCodeAttributes;

[Generator]
public class Generator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var mainSyntaxTree = context.Compilation.SyntaxTrees
                            .First(x => x.HasCompilationUnitRoot);

        var directoryName = Path.GetDirectoryName(mainSyntaxTree.FilePath);
        if (string.IsNullOrEmpty(directoryName))
            return;

        var directory = new DirectoryInfo(directoryName);
        var iterations = 0;
        while (true)
        {
            if (iterations > 10)
                break;

            iterations++;
            
            if (directory == null)
                break;

            if (Directory.Exists(Path.Combine(directory.FullName, ".git")))
                break;
            
            directory = directory.Parent;
        }

        if (directory == null)
            return;

        var (repoOrg, repoName) = GetRepoDetails(directory.FullName);
        var hash = GetCommitHash(directory.FullName);
        
        context.AddSource("Generator", Templates.GetActivitySourceClass(repoOrg, repoName, directory.FullName, hash));
        context.AddSource("GeneratorGlobals", Templates.GenerateGlobalUsing());
    }

    private string GetCommitHash(string solutionPath)
    {
        var headFile = File.ReadAllLines(Path.Combine(solutionPath, ".git", "HEAD"));
        var headRef = headFile[0].Replace("ref: ", "");

        var refsFile = File.ReadAllLines(Path.Combine(solutionPath, ".git", headRef));
        return refsFile[0];
    }

    private (string?, string?) GetRepoDetails(string gitPath)
    {
        var configLines = File.ReadAllLines(Path.Combine(gitPath, ".git", "config"));

        var sections = new Dictionary<string, IniSection>();
        for (int i = 0; i < configLines.Length; i++)
        {
            if (configLines[i].StartsWith("["))
            {
                var section = new IniSection();
                section.Read(configLines, i);
                sections.Add(section.Name, section);
            }
        }

        if (!sections.TryGetValue("[remote \"origin\"]", out var originSection) ||
            originSection == null ||
            !originSection.Properties.ContainsKey("url"))
            return (null, null);

        if (originSection.Properties["url"].StartsWith("git@"))
        {
            var afterColon = originSection.Properties["url"].Split(":")[1];
            var repoOrg = afterColon.Split("/")[0];
            var repoName = afterColon.Split("/")[1].Replace(".git", "");

            return (repoOrg, repoName);
        }

        return (null, null);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }

    private class IniSection
    {
        public string Name { get; private set; } = null!;
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public void Read(string[] lines, int start)
        {
            Name = lines[start];

            for (int i = start + 1; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("["))
                    return;

                var value = lines[i].Split("=");
                Properties.Add(value[0].Trim(), value[1].Trim());
            }
        }
    }
}
