using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class Usings
    {
        private HashSet<string> _usings = new HashSet<string>();
        private string _fileNamespaceName;

        public Context Context { get; }

        public Usings(Context context, string fileNamespaceName)
        {
            Context = context;
            _fileNamespaceName = fileNamespaceName;
        }

        public void AddTypeNamespace(ITypeSymbol type)
        {
            // Built in types don't need a "using System" - it's implicit
            if (Context.GetBuiltInTypeName(type) != null)
                return;

            AddNamespace(Context.GetNamespaceFullName(type.ContainingNamespace));
        }

        public void AddNamespace(string namespaceName)
        {
            // If the file namespace is the same or a child of the specified namespace, it's referenced implicitly
            if (_fileNamespaceName == namespaceName || _fileNamespaceName.StartsWith(namespaceName + "."))
                return;

            _usings.Add(namespaceName);
        }

        public void AddTypeAlias(string alias)
        {
            _usings.Add(alias);
        }

        public Source Generate()
        {
            Source source = new Source(Context);

            foreach (string usingLine in _usings)
            {
                source.AddLine($"using {usingLine};");
            }

            return source;
        }
    }
}
