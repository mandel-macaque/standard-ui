using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.StandardUI.SourceGenerator.UIFrameworks;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class Interface
    {
        public Context Context { get; }
        public INamedTypeSymbol Type { get; }
        public InterfacePurpose Purpose { get; }
        public INamespaceSymbol Namespace { get; }
        public string NamespaceName { get; }
        public string FrameworkClassName { get; }
        public INamedTypeSymbol? AttachedType { get; }
        public string Name { get; }
        public string VariableName { get; }
        public INamedTypeSymbol? LayoutManagerType { get; }
        public INamedTypeSymbol? StandardControlImpelementationType { get; }

        public bool IsThisType(string typeName) => Utils.IsThisType(Type, typeName);

        public bool IsDrawableObject
        {
            get
            {
                if (IsThisType(KnownTypes.ITextBlock))
                    return true;

                if (Type is not INamedTypeSymbol namedType)
                    return false;

                foreach (INamedTypeSymbol intface in namedType.Interfaces)
                {
                    if (Utils.IsThisType(intface, KnownTypes.IShape))
                        return true;
                }

                return false;
            }
        }

        public static InterfacePurpose? IdentifyPurpose(INamedTypeSymbol type)
        {
            // Skip ...Attached interfaces, processing them when their paired main interface is processed instead
            if (type.Name.EndsWith("Attached"))
                return null;

            foreach (AttributeData attribute in type.GetAttributes())
            {
                INamedTypeSymbol? attributeClass = attribute.AttributeClass;
                if (attributeClass == null)
                    continue;

                string attributeTypeFullName = Utils.GetTypeFullName(attributeClass);

                if (attributeTypeFullName == KnownTypes.UIModelAttribute)
                    return InterfacePurpose.StandardUIObject;
                else if (attributeTypeFullName == KnownTypes.StandardPanelAttribute)
                    return InterfacePurpose.StandardPanel;
                else if (attributeTypeFullName == KnownTypes.StandardControlAttribute)
                    return InterfacePurpose.StandardControl;
                else continue;
            }

            return null;
        }

        public Interface(Context context, INamedTypeSymbol type)
        {
            Context = context;

            Type = type;
            Name = type.Name;
            if (!Name.StartsWith("I"))
                throw UserVisibleErrors.StandardUIInterfaceMustStartWithI(type);

            InterfacePurpose? purpose = IdentifyPurpose(type);
            if (! purpose.HasValue)
                throw UserVisibleErrors.StandardUIMissingPurposeAttribute(type);
            Purpose = purpose.Value;

            FrameworkClassName = Name.Substring(1);

            // Form the default variable name for the interface by dropping the "I" and lower casing the first letter(s) after (ICanvas => canvas)
            VariableName = Utils.GetInterfaceVariableName(Type);

            Namespace = Type.ContainingNamespace;
            NamespaceName = Utils.GetNamespaceFullName(Namespace);

            // Get attached type, if it exists
            string fullNameAttached = Utils.GetTypeFullName(type) + "Attached";
            AttachedType = Context.Compilation.GetTypeByMetadataName(fullNameAttached);

            if (Purpose == InterfacePurpose.StandardPanel)
            {
                string layoutManagerFullName = $"{NamespaceName}.{Name.Substring(1)}LayoutManager";
                LayoutManagerType = Context.Compilation.GetTypeByMetadataName(layoutManagerFullName);

                if (LayoutManagerType == null)
                    throw UserVisibleErrors.NoLayoutManagerClassFound(layoutManagerFullName, Name);
            }
            else if (Purpose == InterfacePurpose.StandardControl)
            {
                string standardControlImplementationFullName = $"{NamespaceName}.{Name.Substring(1)}Implementation";
                StandardControlImpelementationType = Context.Compilation.GetTypeByMetadataName(standardControlImplementationFullName);

                if (StandardControlImpelementationType == null)
                    throw UserVisibleErrors.NoLayoutManagerClassFound(standardControlImplementationFullName, Name);
            }
        }

        public void Generate(UIFramework uiFramework)
        {
            string frameworkNamespaceName = uiFramework.ToFrameworkNamespaceName(Namespace);

            var usings = new Usings(Context, frameworkNamespaceName);

            var properties = new List<Property>();

            var stubSourceForUsings = new Source(Context, usings);
            var mainClassStaticFields = new Source(Context, usings);
            var mainClassStaticMethods = new Source(Context, usings);
            var mainClassNonstaticFields = new Source(Context, usings);
            var mainClassNonstaticMethods = new Source(Context, usings);

            var attachedClassStaticFields = new Source(Context, usings);
            var attachedClassMethods = new Source(Context, usings);

            // Add the property descriptors and accessors
            foreach (IPropertySymbol propertySymbol in Type.GetMembers().Where(member => member.Kind == SymbolKind.Property))
            {
                var property = new Property(Context, this, propertySymbol);
                properties.Add(property);

                uiFramework.GeneratePropertyDescriptor(property, mainClassStaticFields);
                uiFramework.GeneratePropertyField(property, mainClassNonstaticFields);
                uiFramework.GeneratePropertyMethods(property, mainClassNonstaticMethods);
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
                            throw UserVisibleErrors.AttachedTypeMethodMustStartWithGetOrSet(AttachedType.Name, methodName);
                        else continue;
                    }

                    string propertyName = methodName.Substring("Get".Length);
                    string setterMethodName = "Set" + propertyName;
                    IMethodSymbol? setterMethod = (IMethodSymbol?) AttachedType.GetMembers(setterMethodName).FirstOrDefault();

                    var attachedProperty = new AttachedProperty(Context, this, AttachedType, getterMethod, setterMethod);

                    uiFramework.GenerateAttachedPropertyDescriptor(attachedProperty, mainClassStaticFields);
                    uiFramework.GenerateAttachedPropertyMethods(attachedProperty, mainClassStaticMethods);
                    uiFramework.GenerateAttachedPropertyAttachedClassMethods(attachedProperty, attachedClassMethods);
                }
            }

            // Add any other methods needed for particular special types
            if (IsDrawableObject)
            {
                uiFramework.GenerateDrawableObjectMethods(this, mainClassNonstaticMethods);
            }

            if (IsThisType(KnownTypes.IPanel))
            {
                uiFramework.GeneratePanelMethods(mainClassNonstaticMethods);
            }

            if (Purpose == InterfacePurpose.StandardPanel)
            {
                uiFramework.GenerateStandardPanelLayoutMethods(LayoutManagerType!.Name, mainClassNonstaticMethods);
            }

            usings.AddTypeNamespace(Type);
            usings.AddNamespace(frameworkNamespaceName);
            Source usingDeclarations = GenerateUsingDeclarations(uiFramework, usings);

            string? destinationBaseClass = GetOutputBaseClass(uiFramework);

            Source? constructor = GenerateConstructor(uiFramework, properties);

            string mainClassDerviedFrom;
            if (destinationBaseClass == null)
                mainClassDerviedFrom = Name;
            else
                mainClassDerviedFrom = $"{destinationBaseClass}, {Name}";

            Source mainClassSource = GenerateClassFile(usings, frameworkNamespaceName, FrameworkClassName, mainClassDerviedFrom,
                constructor: constructor, staticFields: mainClassStaticFields, staticMethods: mainClassStaticMethods, nonstaticFields: mainClassNonstaticFields,
                nonstaticMethods: mainClassNonstaticMethods);
            Context.Output.AddSource(uiFramework, NamespaceName, FrameworkClassName, mainClassSource);

            if (AttachedType != null)
            {
                string attachedClassName = FrameworkClassName + "Attached";
                string attachedClassDerivedFrom = AttachedType.Name;

                attachedClassStaticFields.AddLine($"public static {attachedClassName} Instance = new {attachedClassName}();");

                Source attachedClassSource = GenerateClassFile(usings, frameworkNamespaceName, attachedClassName, attachedClassDerivedFrom,
                    staticFields: attachedClassStaticFields, nonstaticMethods: attachedClassMethods);
                Context.Output.AddSource(uiFramework, NamespaceName, attachedClassName, attachedClassSource);
            }
        }

        public void GenerateExtensionsClass()
        {
            var usings = new Usings(Context, NamespaceName);
            var properties = new List<Property>();
            var methods = new Source(Context, usings);
            var staticFields = new Source(Context, usings);

            // Add interface extension methods, allowing fluent style setters
            foreach (IPropertySymbol propertySymbol in Type.GetMembers().Where(member => member.Kind == SymbolKind.Property))
            {
                var property = new Property(Context, this, propertySymbol);
                properties.Add(property);

                property.GenerateExtensionClassMethods(methods);
            }

            // If there are any attached properties, add target extension methods for those too
            if (AttachedType != null)
            {
                string attachedClassName = FrameworkClassName + "Attached";
                usings.AddNamespace("System");

                staticFields.AddLines(
                    $"private static readonly Lazy<{AttachedType.Name}> s_{attachedClassName} = new Lazy<{AttachedType.Name}>(() => HostEnvironment.Factory.{attachedClassName}Instance);",
                    $"public static {AttachedType.Name} {attachedClassName}Instance => s_{attachedClassName}.Value;");

                methods.AddBlankLineIfNonempty();
                methods.AddLine("// Attached properties");

                foreach (ISymbol member in AttachedType.GetMembers())
                {
                    if (!(member is IMethodSymbol getterMethod))
                        continue;

                    // We just process the Get 
                    string methodName = getterMethod.Name;
                    if (!methodName.StartsWith("Get"))
                    {
                        if (!methodName.StartsWith("Set"))
                            throw UserVisibleErrors.AttachedTypeMethodMustStartWithGetOrSet(AttachedType.Name, methodName);
                        else continue;
                    }

                    string propertyName = methodName.Substring("Get".Length);
                    string setterMethodName = "Set" + propertyName;
                    IMethodSymbol? setterMethod = (IMethodSymbol?)AttachedType.GetMembers(setterMethodName).FirstOrDefault();

                    var attachedProperty = new AttachedProperty(Context, this, AttachedType, getterMethod, setterMethod);

                    attachedProperty.GenerateExtensionClassMethods(methods);
                }
            }

            if (!(methods.IsEmpty && staticFields.IsEmpty))
            {
                string extensionsClassName = FrameworkClassName + "Extensions";
                Source extensionsClassSource = GenerateStaticClassFile(usings, NamespaceName, extensionsClassName, methods, staticFields);
                Context.Output.AddSource(null, NamespaceName, extensionsClassName, extensionsClassSource);
            }
        }

        public Source GenerateClassFile(Usings usings, string namespaceName, string className, string derivedFrom, bool isPartial = false,
            Source? constructor = null, Source? staticFields = null, Source? staticMethods = null, Source? nonstaticFields = null, Source? nonstaticMethods = null)
        {
            Source fileSource = new Source(Context);

            GenerateFileHeader(fileSource);

            Source usingsDeclarations = usings.Generate();
            if (!usingsDeclarations.IsEmpty)
            {
                fileSource.AddSource(usingsDeclarations);
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

        public Source GenerateStaticClassFile(Usings usings, string namespaceName, string className, Source staticMethods, Source? staticFields = null)
        {
            Source fileSource = new Source(Context);

            GenerateFileHeader(fileSource);

            Source usingDeclarations = usings.Generate();
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
                    if (staticFields != null && !staticFields.IsEmpty)
                    {
                        fileSource.AddSource(staticFields);
                        fileSource.AddBlankLine();
                    }

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

        private Source? GenerateConstructor(UIFramework uiFramework, List<Property> properties)
        {
            Source constructorBody = new Source(Context);
            foreach (Property property in properties)
                uiFramework.GeneratePropertyInit(property, constructorBody);

            if (Purpose == InterfacePurpose.StandardControl)
            {
                string implementationFullTypeName = Utils.GetTypeFullName(StandardControlImpelementationType);
                constructorBody.AddLine(
                    $"InitImplementation(new {implementationFullTypeName}(this));");
            }

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

        private Source GenerateUsingDeclarations(UIFramework uiFramework, Usings usings)
        {
#if false
            foreach (var member in Declaration.Members)
            {
                if (!(member is PropertyDeclarationSyntax modelProperty))
                    continue;

                // Array.Empty requires System
                if (modelProperty.Type is ArrayTypeSyntax)
                    AddUsing(usings, IdentifierName("System"));
            }
#endif

            /*
            foreach (IPropertySymbol propertySymbol in Type.GetMembers().Where(member => member.Kind == SymbolKind.Property))
            {
                var property = new Property(Context, this, propertySymbol);
                OutputType.AddTypeAliasUsingIfNeeded(usings, property.FrameworkTypeName.ToString());
            }
            */

            return usings.Generate();
        }

        private string? GetOutputBaseClass(UIFramework uiFramework)
        {
#if NOMORE
            string? elementType = Utils.IsCollectionType(Type);
            if (elementType != null)
                return $"StandardUICollection<{elementType}>";
#endif

            INamedTypeSymbol? baseInterface = Type.Interfaces.FirstOrDefault();

            if (baseInterface == null)
                return null;
            else
                return uiFramework.OutputTypeName(baseInterface);
        }
    }
}
