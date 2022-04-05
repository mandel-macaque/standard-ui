using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.StandardUI.SourceGenerator.UIFrameworks;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class GeneratorExecutionOutput : Output
    {
        public GeneratorExecutionContext GeneratorExecutionContext { get; }

        public GeneratorExecutionOutput(GeneratorExecutionContext generatorExecutionContext)
        {
            GeneratorExecutionContext = generatorExecutionContext;
        }

        public override void AddSource(UIFramework? uiFramework, string? childNamespaceName, string fileName, Source source)
        {
            StringWriter stringWriter = new StringWriter();
            stringWriter.Write(source);
            string sourceString = stringWriter.ToString();

            GeneratorExecutionContext.AddSource(fileName + ".cs", sourceString);
        }
    }
}
