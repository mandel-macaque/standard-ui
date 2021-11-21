using System.IO;
using Microsoft.CodeAnalysis;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class Context
    {
        public const string RootNamespace = "Microsoft.StandardUI";
        public static readonly SymbolDisplayFormat TypeFullNameSymbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

        public int IndentSize { get; } = 4;
        public Compilation Compilation { get; }
        public string RootDirectory { get; }

        public Context(Compilation compilation, string rootDirectory)
        {
            Compilation = compilation;
            RootDirectory = rootDirectory;
        }

        public string GetSharedOutputDirectory(string? childNamespaceName)
        {
            string outputDirectory = Path.Combine(RootDirectory, "src", "StandardUI", "generated");
            if (childNamespaceName != null)
                outputDirectory = Path.Combine(outputDirectory, childNamespaceName);

            return outputDirectory;
        }
    }
}
