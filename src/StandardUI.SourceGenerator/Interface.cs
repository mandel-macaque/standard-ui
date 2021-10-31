using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class Interface
    {
        private readonly CompilationUnitSyntax _sourceCompilationUnit;

        public Context Context { get; }
        public INamedTypeSymbol Type { get; }
        public INamespaceSymbol Namespace { get; }
        public string NamespaceName { get; }
        public string? ChildNamespaceName { get; }
        public string FrameworkClassName { get; }
        public string FrameworkNamespaceName { get; }
        public InterfaceDeclarationSyntax Declaration { get; }
        public InterfaceDeclarationSyntax? AttachedInterfaceDeclaration { get; }
        public INamedTypeSymbol? AttachedType { get; }
        public string Name { get; }
        public string VariableName { get; }

        public Interface(Context context, InterfaceDeclarationSyntax sourceInterfaceDeclaration, InterfaceDeclarationSyntax? sourceAttachedInterfaceDeclaration)
        {
            Context = context;
            Declaration = sourceInterfaceDeclaration;
            AttachedInterfaceDeclaration = sourceAttachedInterfaceDeclaration;

            Name = sourceInterfaceDeclaration.Identifier.Text;
            if (!Name.StartsWith("I"))
                throw new UserViewableException($"Data model interface {Name} must start with 'I'");


            if (!(sourceInterfaceDeclaration.Parent is NamespaceDeclarationSyntax interfaceNamespaceDeclaration))
                throw new UserViewableException(
                    $"Parent of ${sourceInterfaceDeclaration.Identifier.Text} interface should be namespace declaration, but it's a {sourceInterfaceDeclaration.Parent.GetType()} node instead");
            string namespaceName = interfaceNamespaceDeclaration.Name.ToString();
            string fullName = namespaceName + "." + sourceInterfaceDeclaration.Identifier.ToString();
            INamedTypeSymbol? interfaceSymbol = Context.Compilation.GetTypeByMetadataName(fullName);
            if (interfaceSymbol == null)
                throw new UserViewableException($"No semantic data found for type {fullName}");
            Type = interfaceSymbol;


            FrameworkClassName = Name.Substring(1);

            // Form the default variable name for the interface by dropping the "I" and lower casing the first letter(s) after (ICanvas => canvas)
            VariableName = Context.TypeNameToVariableName(Name.Substring(1));

            Namespace = Type.ContainingNamespace;
            NamespaceName = Context.GetNamespaceFullName(Namespace);
            ChildNamespaceName = Context.GetChildNamespaceName(namespaceName);

            if (!(interfaceNamespaceDeclaration.Parent is CompilationUnitSyntax compilationUnit))
                throw new UserViewableException(
                    $"Parent of ${interfaceNamespaceDeclaration} namespace should be compilation unit, but it's a {interfaceNamespaceDeclaration.Parent!.GetType()} node instead");
            _sourceCompilationUnit = compilationUnit;

            FrameworkNamespaceName = Context.ToFrameworkNamespaceName(Namespace);

            if (sourceAttachedInterfaceDeclaration != null)
            {
                string fullNameAttached = namespaceName + "." + sourceAttachedInterfaceDeclaration.Identifier.Text;
                INamedTypeSymbol? interfaceSymbolAttached = context.Compilation.GetTypeByMetadataName(fullNameAttached);
                if (interfaceSymbolAttached == null)
                    throw new UserViewableException($"No semantic data found for attached type {fullNameAttached}");
                AttachedType = interfaceSymbolAttached;
            }
        }

        public void Generate()
        {
            var properties = new List<Property>();

            var mainClassStaticFields = new Source(Context);
            var mainClassStaticMethods = new Source(Context);
            var mainClassNonstaticFields = new Source(Context);
            var mainClassNonstaticMethods = new Source(Context);

            var extensionClassMethods = new Source(Context);
            var attachedClassStaticFields = new Source(Context);
            var attachedClassMethods = new Source(Context);

            // Add the property descriptors and accessors
            foreach (IPropertySymbol propertySymbol in Type.GetMembers().Where(member => member.Kind == SymbolKind.Property))
            {
                PropertyDeclarationSyntax propertyDeclaration = (PropertyDeclarationSyntax)Declaration.Members.Where(
                    member => member is PropertyDeclarationSyntax memberProperty && memberProperty.Identifier.Text == propertySymbol.Name).First();

                var property = new Property(Context, this, propertySymbol);
                properties.Add(property);

                property.GenerateDescriptor(mainClassStaticFields);
                property.GenerateFieldIfNeeded(mainClassNonstaticFields);
                property.GenerateMethods(mainClassNonstaticMethods);
                property.GenerateExtensionClassMethods(extensionClassMethods);
            }

            if (Context.IncludeDraw(Declaration))
            {
                mainClassNonstaticMethods.AddBlankLineIfNonempty();
                mainClassNonstaticMethods.AddLine(
                    $"public override void Draw(IDrawingContext drawingContext) => drawingContext.Draw{FrameworkClassName}(this);");
            }

            // Add a special case for the WPF visual tree child methods for Panel; later we'll generalize this as needed
            if (Name == "IPanel" && Context.OutputType is WpfXamlOutputType)
            {
                mainClassNonstaticMethods.AddBlankLineIfNonempty();

                mainClassNonstaticMethods.AddLine(
                    "protected override int VisualChildrenCount => _uiElementCollection.Count;");
                mainClassNonstaticMethods.AddBlankLine();
                mainClassNonstaticMethods.AddLine(
                    "protected override System.Windows.Media.Visual GetVisualChild(int index) => (System.Windows.Media.Visual) _uiElementCollection[index];");
            }

            // If there are any attached properties, add the property descriptors and accessors for them
            if (AttachedType != null)
            {
                foreach (ISymbol member in AttachedType.GetMembers())
                {
                    if (!(member is IMethodSymbol getterMethod))
                        continue;

                    // We just process the Get 
                    string methodName = getterMethod.Name;
                    if (!methodName.StartsWith("Get"))
                    {
                        if (!methodName.StartsWith("Set"))
                            throw new UserViewableException(
                                $"Attached type method {AttachedInterfaceDeclaration.Identifier.Text}.{methodName} doesn't start with Get or Set");
                        else continue;
                    }

                    string propertyName = methodName.Substring("Get".Length);
                    string setterMethodName = "Set" + propertyName;
                    IMethodSymbol? setterMethod = (IMethodSymbol?) AttachedType.GetMembers(setterMethodName).FirstOrDefault();

                    var attachedProperty = new AttachedProperty(Context, this, AttachedType, getterMethod, setterMethod);


                    attachedProperty.GenerateMainClassDescriptor(mainClassStaticFields);
                    attachedProperty.GenerateMainClassMethods(mainClassStaticMethods);
                    attachedProperty.GenerateAttachedClassMethods(attachedClassMethods);
                }
            }

            Source usingDeclarations = GenerateUsingDeclarations(!mainClassStaticFields.IsEmpty);

            string? destinationBaseClass = GetDestinationBaseClass();

            Source? constructor = GenerateConstructor(properties);

            string platformOutputDirectory = Context.GetPlatformOutputDirectory(ChildNamespaceName);

            string mainClassDerviedFrom;
            if (destinationBaseClass == null)
                mainClassDerviedFrom = Name;
            else
                mainClassDerviedFrom = $"{destinationBaseClass}, {Name}";

            bool isPartial = Context.IsPanelSubclass(Declaration);

            Source mainClassSource = GenerateClassFile(usingDeclarations, FrameworkNamespaceName, FrameworkClassName, mainClassDerviedFrom, isPartial: isPartial,
                constructor: constructor, staticFields: mainClassStaticFields, staticMethods: mainClassStaticMethods, nonstaticFields: mainClassNonstaticFields,
                nonstaticMethods: mainClassNonstaticMethods);
            mainClassSource.WriteToFile(platformOutputDirectory, FrameworkClassName + ".cs");

            if (AttachedInterfaceDeclaration != null)
            {
                string attachedClassName = FrameworkClassName + "Attached";
                string attachedClassDerivedFrom = AttachedInterfaceDeclaration.Identifier.Text;

                attachedClassStaticFields.AddLine($"public static {attachedClassName} Instance = new {attachedClassName}();");

                Source attachedClassSource = GenerateClassFile(usingDeclarations, FrameworkNamespaceName, attachedClassName, attachedClassDerivedFrom,
                    staticFields: attachedClassStaticFields, nonstaticMethods: attachedClassMethods);
                attachedClassSource.WriteToFile(platformOutputDirectory, attachedClassName + ".cs");
            }

            if (!extensionClassMethods.IsEmpty)
            {
                string extensionsClassName = FrameworkClassName + "Extensions";
                Source extensionsClassSource = GenerateStaticClassFile(GenerateExtensionsClassUsingDeclarations(), NamespaceName, extensionsClassName, extensionClassMethods);
                extensionsClassSource.WriteToFile(Context.GetSharedOutputDirectory(ChildNamespaceName), extensionsClassName + ".cs");
            }
        }

        public Source GenerateClassFile(Source usingDeclarations, string namespaceName, string className, string derivedFrom, bool isPartial = false,
            Source? constructor = null, Source? staticFields = null, Source? staticMethods = null, Source? nonstaticFields = null, Source? nonstaticMethods = null)
        {
            Source fileSource = new Source(Context);

            GenerateFileHeader(fileSource);

            if (!usingDeclarations.IsEmpty)
            {
                fileSource.AddSource(usingDeclarations);
                fileSource.AddBlankLine();
            }

            fileSource.AddLines(
                $"namespace {namespaceName}",
                "{");

            using (fileSource.Indent())
            {
                Source classBody = new Source(Context);
                if (staticFields != null && !staticFields.IsEmpty)
                    classBody.AddSource(staticFields);
                if (staticMethods != null && !staticMethods.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(staticMethods);
                }
                if (nonstaticFields != null && !nonstaticFields.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(nonstaticFields);
                }
                if (constructor != null && !constructor.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(constructor);
                }
                if (nonstaticMethods != null && !nonstaticMethods.IsEmpty)
                {
                    classBody.AddBlankLineIfNonempty();
                    classBody.AddSource(nonstaticMethods);
                }

                string modifiers = isPartial ? "public partial" : "public";
                fileSource.AddLines(
                    $"{modifiers} class {className} : {derivedFrom}",
                    "{");
                using (fileSource.Indent())
                    fileSource.AddSource(classBody);
                fileSource.AddLine(
                    "}");
            }

            fileSource.AddLine(
                "}");

            return fileSource;
        }

        public Source GenerateStaticClassFile(Source usingDeclarations, string namespaceName, string className, Source staticMethods)
        {
            Source fileSource = new Source(Context);

            GenerateFileHeader(fileSource);

            if (!usingDeclarations.IsEmpty)
            {
                fileSource.AddSource(usingDeclarations);
                fileSource.AddBlankLine();
            }

            fileSource.AddLines(
                $"namespace {namespaceName}",
                "{");

            using (fileSource.Indent())
            {
                fileSource.AddLines(
                    $"public static class {className}",
                    "{");
                using (fileSource.Indent())
                {
                    fileSource.AddSource(
                        staticMethods);
                }
                fileSource.AddLine(
                    "}");
            }

            fileSource.AddLine(
                "}");

            return fileSource;
        }

        private void GenerateFileHeader(Source fileSource)
        {
            fileSource.AddLine($"// This file is generated from {Name}.cs. Update the source file to change its contents.");
            fileSource.AddBlankLine();
        }

        public OutputType OutputType => Context.OutputType;

        private Source? GenerateConstructor(List<Property> collectionProperties)
        {
            Source constructorBody = new Source(Context);
            foreach (Property property in collectionProperties)
                property.GenerateConstructorLinesIfNeeded(constructorBody);

            if (constructorBody.IsEmpty)
                return null;

            Source constructor = new Source(Context);
            constructor.AddLines(
                $"public {FrameworkClassName}()",
                "{");
            using (constructor.Indent())
                constructor.AddSource(
                    constructorBody);
            constructor.AddLine(
                "}");

            return constructor;
        }

        private Source GenerateUsingDeclarations(bool hasPropertyDescriptors)
        {
            Source source = new Source(Context);

            var usings = new HashSet<string>();

            foreach (UsingDirectiveSyntax sourceUsing in _sourceCompilationUnit.Usings)
            {
                NameSyntax sourceUsingName = sourceUsing.Name;
                AddUsing(usings, sourceUsingName);

                if (sourceUsingName.ToString().StartsWith("Microsoft.StandardUI."))
                    AddUsing(usings, Context.ToFrameworkNamespaceName(sourceUsingName));
            }

            AddUsing(usings, NamespaceName);

            OutputType.AddUsings(usings, hasPropertyDescriptors, DestinationTypeHasTypeConverterAttribute());

            foreach (var member in Declaration.Members)
            {
                if (!(member is PropertyDeclarationSyntax modelProperty))
                    continue;

                // Array.Empty requires System
                if (modelProperty.Type is ArrayTypeSyntax)
                    AddUsing(usings, IdentifierName("System"));
            }

            if (DestinationTypeHasTypeConverterAttribute())
                AddUsing(usings, OutputType.RootNamespace + ".Converters");

            foreach (IPropertySymbol propertySymbol in Type.GetMembers().Where(member => member.Kind == SymbolKind.Property))
            {
                PropertyDeclarationSyntax propertyDeclaration = (PropertyDeclarationSyntax)Declaration.Members.Where(
                    member => member is PropertyDeclarationSyntax memberProperty && memberProperty.Identifier.Text == propertySymbol.Name).First();

                var property = new Property(Context, this, propertySymbol);
                OutputType.AddTypeAliasUsingIfNeeded(usings, property.FrameworkTypeName.ToString());
            }

            foreach (string @using in usings)
            {
                source.AddLine($"using {@using};");
            }

            return source;
        }

        private Source GenerateExtensionsClassUsingDeclarations()
        {
            Source source = new Source(Context);

            foreach (UsingDirectiveSyntax sourceUsing in _sourceCompilationUnit.Usings)
            {
                source.AddLine($"using {sourceUsing.Name};");
            }

            return source;
        }

        private static void AddUsing(HashSet<string> usings, NameSyntax name) => usings.Add(name.ToString());

        private static void AddUsing(HashSet<string> usings, string @using) => usings.Add(@using);

        private bool DestinationTypeHasTypeConverterAttribute()
        {
            return Context.OutputType is XamlOutputType &&
                   (FrameworkClassName == "Geometry" || FrameworkClassName == "Brush");
        }

        private string? GetDestinationBaseClass()
        {
            string? elementType = Context.IsCollectionType(Declaration.Identifier.Text);
            if (elementType != null)
                return $"StandardUICollection<{elementType}>";

            INamedTypeSymbol? baseInterface = Type.Interfaces.FirstOrDefault();

            if (baseInterface == null)
                return OutputType.DefaultBaseClassName;
            else
                return Context.ToFrameworkTypeName(baseInterface);
        }
    }
}
