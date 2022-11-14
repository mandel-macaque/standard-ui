using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class ControlLibrary
    {
        public Context Context { get; }
        public string LibraryNamespace { get; }
        public string LibraryName { get; }
        public List<Interface> Interfaces { get; }

        public ControlLibrary(Context context, string fullyQualifiedName, SyntaxReference? attributeSyntaxReference,
            List<Interface> interfaces)
        {
            Context = context;
            Interfaces = interfaces;

            int lastPeriod = fullyQualifiedName.LastIndexOf(".");
            if (lastPeriod == -1)
                throw UserVisibleErrors.ControlLibraryAttributeInvalid(attributeSyntaxReference);

            LibraryNamespace = fullyQualifiedName.Substring(0, lastPeriod);
            LibraryName = fullyQualifiedName.Substring(lastPeriod + 1);
        }

        public void GenerateFactoryClass()
        {
            var factoryClassSource = new ClassSource(Context,
                namespaceName: LibraryNamespace,
                className: $"{LibraryName}Factory",
                isStatic: true);

            Usings usings = factoryClassSource.Usings;
            Source members = factoryClassSource.StaticMethods;

            usings.AddNamespace("System");
            usings.AddNamespace("Microsoft.StandardUI");

            members.AddLines(
                "private static Func<T> UninitializedCreator<T>() =>");
            using (members.Indent())
            {
                members.AddLines(
                    $"() => throw new FactoryNotInitializedException(\"{LibraryName}\");");
            }
            members.AddBlankLine();

            bool anySingletons = false;
            foreach (Interface intface in Interfaces)
            {
                if (intface.Purpose == InterfacePurpose.UISingleton)
                {
                    anySingletons = true;
                    continue;
                }

                usings.AddNamespace(intface.NamespaceName);
                members.AddLine(
                    $"public static Func<{intface.Name}> {intface.FrameworkClassName}Creator {{ get; set; }} = UninitializedCreator<{intface.Name}>();");
            }

            if (anySingletons)
            {
                members.AddBlankLine();
                members.AddLine("// Singletons");

                foreach (Interface intface in Interfaces)
                {
                    if (intface.Purpose != InterfacePurpose.UISingleton)
                    {
                        continue;
                    }

                    usings.AddNamespace(intface.NamespaceName);
                    members.AddLine(
                        $"public static {intface.Name} {intface.FrameworkClassName} {{ get; set; }}");
                }
            }

            factoryClassSource.AddToOutput(null);
        }

        public void GenerateStaticsClass()
        {
            var factoryClassSource = new ClassSource(Context,
                namespaceName: LibraryNamespace,
                className: $"{LibraryName}Statics",
                isStatic: true);

            Usings usings = factoryClassSource.Usings;
            Source members = factoryClassSource.StaticMethods;

            bool anySingletons = false;
            foreach (Interface intface in Interfaces)
            {
                if (intface.Purpose == InterfacePurpose.UISingleton)
                {
                    anySingletons = true;
                    continue;
                }

                usings.AddNamespace(intface.NamespaceName);
                members.AddLine(
                    $"public static {intface.Name} {intface.FrameworkClassName}() => {LibraryName}Factory.{intface.FrameworkClassName}Creator();");
            }

            if (anySingletons)
            {
                members.AddBlankLine();
                members.AddLine("// Singletons");

                foreach (Interface intface in Interfaces)
                {
                    if (intface.Purpose != InterfacePurpose.UISingleton)
                    {
                        continue;
                    }

                    members.AddLine(
                        $"public static {intface.Name} {intface.FrameworkClassName} => {LibraryName}Factory.{intface.FrameworkClassName};");
                }
            }

            factoryClassSource.AddToOutput(null);
        }
    }
}
