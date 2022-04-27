using Microsoft.CodeAnalysis;

namespace ActivitySourceGenerator;

[Generator]
public class Generator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        context.AddSource("Generator", Templates.GetActivitySourceClass("orgname", "reponame", "/home/martin", ""));
        context.AddSource("GeneratorGlobals", Templates.GenerateGlobalUsing());
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }
}
