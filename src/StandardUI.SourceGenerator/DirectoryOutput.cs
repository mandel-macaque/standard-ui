using Microsoft.StandardUI.SourceGenerator.UIFrameworks;
using System.IO;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class DirectoryOutput : Output
    {
        public string RootDirectory { get; }

        public DirectoryOutput(string rootDirectory)
        {
            RootDirectory = rootDirectory;
        }

        public override void AddSource(UIFramework? uiFramework, string? childNamespaceName, string fileName, Source source)
        {
            string outputDirectory;
            if (uiFramework != null)
            {
                outputDirectory = Path.Combine(RootDirectory, "src", uiFramework.ProjectBaseDirectory, "generated");
                if (childNamespaceName != null)
                    outputDirectory = Path.Combine(outputDirectory, childNamespaceName);
            }
            else
            {
                outputDirectory = Path.Combine(RootDirectory, "src", "StandardUI", "generated");
                if (childNamespaceName != null)
                    outputDirectory = Path.Combine(outputDirectory, childNamespaceName);
            }

            source.WriteToFile(outputDirectory, fileName + ".cs");
        }
    }
}
