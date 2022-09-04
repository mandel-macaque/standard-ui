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

        public ControlLibrary(Context context, AttributeData libraryAttribute, List<Interface> interfaces)
        {
            Context = context;
            Interfaces = interfaces;

            ImmutableArray<TypedConstant> constructorArguments = libraryAttribute.ConstructorArguments;
            if (constructorArguments.Length != 1)
                throw UserVisibleErrors.ControlLibraryAttributeInvalid(libraryAttribute);

            if (constructorArguments[0].Value is not string fullyQualifiedName)
                throw UserVisibleErrors.ControlLibraryAttributeInvalid(libraryAttribute);

            int lastPeriod = fullyQualifiedName.LastIndexOf(".");
            if (lastPeriod == -1)
                throw UserVisibleErrors.ControlLibraryAttributeInvalid(libraryAttribute);

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

            members.AddLines(
                "private static Func<T> UninitializedCreator<T>() =>");
            using (members.Indent())
            {
                members.AddLines(
                    $"() => throw new FactoryNotInitializedException(\"{LibraryName}\");");
            }
            members.AddBlankLine();

            foreach (Interface intface in Interfaces)
            {
                usings.AddNamespace(intface.NamespaceName);
                members.AddLine(
                    $"public static Func<{intface.Name}> {intface.FrameworkClassName}Creator {{ get; set; }} = UninitializedCreator<{intface.Name}>();");
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

            foreach (Interface intface in Interfaces)
            {
                usings.AddNamespace(intface.NamespaceName);
                members.AddLine(
                    $"public static {intface.Name} {intface.FrameworkClassName} => {LibraryName}Factory.{intface.FrameworkClassName}Creator();");
            }

            factoryClassSource.AddToOutput(null);
        }
    }
}
