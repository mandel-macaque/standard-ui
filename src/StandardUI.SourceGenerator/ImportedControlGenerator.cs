using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.StandardUI.SourceGenerator.UIFrameworks;

namespace Microsoft.StandardUI.SourceGenerator
{
    /// <summary>
    /// Generates framework-specific implementations of StandardUI control interfaces. The
    /// consuming app specifies the control interfaces that it consumes via StandardUIControl
    /// assembly attributes.
    /// </summary>
    /// <example>
    /// [assembly: StandardUIControl("Namespace.IControlName")]
    /// </example>
    [Generator]
    internal class ImportedControlGenerator : ISourceGenerator
    {
        private const string AttributeSource = @"// This file was generated

[System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple=true)]
internal sealed class ImportStandardControlAttribute : System.Attribute
{
    public string TypeName { get; }
    public ImportStandardControlAttribute(string typeName)
    {
        TypeName = typeName;
    }
}
";

        public void Initialize(GeneratorInitializationContext context)
        {
#if false
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif 
            
            context.RegisterForPostInitialization((pi) => pi.AddSource("StandardUI_AssemblyAttributes", AttributeSource));
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            SyntaxReceiver rx = (SyntaxReceiver)context.SyntaxContextReceiver!;

            HashSet<string> generatedInterfaces = new HashSet<string>();
            foreach (string interfaceFullTypeName in rx.InterfacesToGenerate)
            {
                INamedTypeSymbol? interfaceSymbol = context.Compilation.GetTypeByMetadataName(interfaceFullTypeName);
                if (interfaceSymbol == null)
                {
                    continue;
                }

                if (generatedInterfaces.Contains(interfaceFullTypeName))
                {
                    continue;
                }

                ImportedControlGenerator.GenerateSourceFile(context, interfaceFullTypeName);
                generatedInterfaces.Add(interfaceFullTypeName);

                // Generate any ancestor types
                INamedTypeSymbol? ancestorType = GetBaseInterface(interfaceSymbol);
                while (ancestorType != null)
                {
                    var symbolDisplayFormat = new SymbolDisplayFormat(
                        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
                    string ancestorFullTypeName = ancestorType.ToDisplayString(symbolDisplayFormat);

                    if (ancestorFullTypeName == "Microsoft.StandardUI.Controls.IStandardControl" || generatedInterfaces.Contains(ancestorFullTypeName))
                        break;

                    ImportedControlGenerator.GenerateSourceFile(context, ancestorFullTypeName);
                    generatedInterfaces.Add(ancestorFullTypeName);

                    ancestorType = GetBaseInterface(ancestorType);
                }
            }
        }

        private static UIFramework GetUIFramework(Compilation compilation, Context context)
        {
            foreach (AssemblyIdentity referencedAssembly in compilation.ReferencedAssemblyNames)
            {
                string displayName = referencedAssembly.GetDisplayName();

                if (displayName == "StandardUI.Wpf")
                    return new WpfUIFramework(context);
                else if (displayName == "StandardUI.WinForms")
                    return new WinFormsUIFramework(context);
            }
            
            throw new InvalidOperationException("Could not identify UI framework type; add reference to the platform's StandardUI assembly to specify");
        }

        private static void GenerateSourceFile(GeneratorExecutionContext generatorExecutionContext, string interfaceFullTypeName)
        {
            Compilation compilation = generatorExecutionContext.Compilation;

            INamedTypeSymbol? interfaceSymbol = compilation.GetTypeByMetadataName(interfaceFullTypeName);
            if (interfaceSymbol == null)
            {
                return;
            }

            if (!TryGetTypeNamesFromInterface(interfaceFullTypeName, out string interfaceNamespace, out string controlTypeName))
            {
                return;
            }

            Context context = new Context(compilation, new GeneratorExecutionOutput(generatorExecutionContext));
            UIFramework uiFramework = GetUIFramework(compilation, context);

            var intface = new Interface(context, interfaceSymbol);
            intface.Generate(uiFramework);

#if false
            string baseTypeName = GetBaseInterface(interfaceSymbol).Name;
            string controlBaseTypeName = baseTypeName == "IStandardControl" ? "StandardControl" : baseTypeName.Substring(1);

            StringBuilder sourceCode = new StringBuilder();
            sourceCode.Append($@"// This file was generated

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Wpf.Media;
using Microsoft.StandardUI.Wpf;

namespace {interfaceNamespace}.Wpf
{{
    public class {controlTypeName} : {controlBaseTypeName}, {interfaceFullTypeName}
    {{
        public {controlTypeName}()
        {{
            InitImplementation(new {interfaceNamespace}.{controlTypeName}Implementation<{interfaceFullTypeName}>(this));
        }}");

            ImportedControlGenerator.GenerateProperties(interfaceSymbol, controlTypeName, sourceCode);

            sourceCode.Append($@"
    }}
}}");

            // Create the file
            generatorExecutionContext.AddSource(controlTypeName, sourceCode.ToString());
#endif
        }

        private static void GenerateChartSourceFile(GeneratorExecutionContext context)
        {
            string controlTypeName = "Chart";

            string sourceCode = @"// This file was generated

using Microsoft.StandardUI.Wpf;
using System.Collections.Generic;
using Microsoft.StandardUI;

namespace Microcharts.Wpf
{
    public class Chart : StandardControl, IChart
    {
        public static readonly System.Windows.DependencyProperty ChartTypeProperty = PropertyUtils.Register(nameof(ChartType), typeof(ChartType), typeof(Chart), ChartType.BarChart);
        public static readonly System.Windows.DependencyProperty EntriesProperty = PropertyUtils.Register(nameof(Entries), typeof(IEnumerable<ChartEntry>), typeof(Chart), null);
        public static readonly System.Windows.DependencyProperty BackgroundColorProperty = PropertyUtils.Register(nameof(BackgroundColor), typeof(ColorWpf), typeof(Chart), ColorWpf.Default);
        public static readonly System.Windows.DependencyProperty LabelColorProperty = PropertyUtils.Register(nameof(LabelColor), typeof(ColorWpf), typeof(Chart), ColorWpf.Default);

        public Chart()
        {
            InitImplementation(new Microcharts.ChartImplementation(this));
        }

        public ChartType ChartType
        {
            get => (ChartType) GetValue(ChartTypeProperty);
            set => SetValue(ChartTypeProperty, value);
        }
        
        public IEnumerable<ChartEntry> Entries
        {
            get => (IEnumerable<ChartEntry>)GetValue(EntriesProperty);
            set => SetValue(EntriesProperty, value);
        }

        public ColorWpf BackgroundColor
        {
            get => (ColorWpf)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        Color IChart.BackgroundColor
        {
            get => BackgroundColor.Color;
            set => BackgroundColor = new ColorWpf(value);
        }

        public ColorWpf LabelColor
        {
            get => (ColorWpf)GetValue(LabelColorProperty);
            set => SetValue(LabelColorProperty, value);
        }
        Color IChart.LabelColor
        {
            get => LabelColor.Color;
            set => LabelColor = new ColorWpf(value);
        }
    }
}
";

            // Create the file
            context.AddSource(controlTypeName, sourceCode);
        }

        /// <summary>
        /// Return the first base interface or null if there aren't any
        /// </summary>
        private static INamedTypeSymbol? GetBaseInterface(INamedTypeSymbol interfaceSymbol)
        {
            foreach (INamedTypeSymbol baseInterface in interfaceSymbol.Interfaces)
            {
                return baseInterface;
            }

            return null;
        }

        /// <summary>
        /// Appends the source code to implement the properties on <paramref name="interfaceSymbol"/> to <paramref name="sourceCode"/>.
        /// </summary>
        /// <example>
        /// public Brush? Fill
        /// {
        ///     get => (Brush?)GetValue(FillProperty);
        ///     set => SetValue(FillProperty, value);
        /// }
        ///
        /// IBrush IRadialGauge.Fill
        /// {
        ///     get => Fill;
        ///     set => Fill = (Brush?)value;
        /// }
        /// </example>
        /// <param name="interfaceSymbol">The interface whose properties should be implemented.</param>
        /// <param name="controlTypeName">The name of the control class that is implementing the interface.</param>
        /// <param name="sourceCode">The StringBuilder to which the source code should be written.</param>
        private static void GenerateProperties(INamedTypeSymbol interfaceSymbol, string controlTypeName, StringBuilder sourceCode)
        {
            foreach (IPropertySymbol propertySymbol in interfaceSymbol.GetMembers().Where(member => member.Kind == SymbolKind.Property))
            {
                string propertyName = propertySymbol.Name;
                string explicitInterfacePropertyType = propertySymbol.Type.Name;
                string publicPropertyType = ImportedControlGenerator.IsStandardUIInterface(propertySymbol.Type) ? explicitInterfacePropertyType.Substring(1) : explicitInterfacePropertyType;

                // The dependency property
                sourceCode.Append($@"

        public static readonly System.Windows.DependencyProperty {propertyName}Property = PropertyUtils.Register(nameof({propertyName}), typeof({publicPropertyType}), typeof({controlTypeName}), null);
");

                // The public property implementation
                sourceCode.Append($@"
        public {publicPropertyType}? {propertyName}
        {{
            get => ({publicPropertyType}?)GetValue({propertyName}Property);");

                if (propertySymbol.SetMethod != null)
                {
                    sourceCode.Append($@"
            set => SetValue({propertyName}Property, value);");
                }

                sourceCode.Append($@"
        }}
");

                // The explicit property implementation
                sourceCode.Append($@"
        {explicitInterfacePropertyType} {propertySymbol.ContainingType.Name}.{propertyName}
        {{
            get => {propertyName};");

                if (propertySymbol.SetMethod != null)
                {
                    sourceCode.Append($@"
            set => { propertyName} = ({publicPropertyType}?)value;");
                }

                sourceCode.Append($@"
        }}");
            }
        }

        /// <summary>
        /// Returns true if the Roslyn symbol represents a Standard UI control interface, otherwise false.
        /// </summary>
        private static bool IsStandardUIInterface(ITypeSymbol type)
        {
            string typeName = type.Name;

            return typeName.Length > 1 && typeName[0] == 'I' && typeName[1] == char.ToUpper(typeName[1]);
        }

        /// <summary>
        /// Given the full name (with namespace) of an interface type, extracts various other related strings.
        /// </summary>
        /// <param name="interfaceFullTypeName">The full name (with namespace) of an interface type. For example, Contoso.Controls.IControl</param>
        /// <param name="interfaceNamespace">The interface's namespace. For example, Contoso.Controls</param>
        /// <param name="controlTypeName">The default name of the class implementing the interface (by convention). For example, Control</param>
        /// <returns>True if the output strings were successfully determined, otherwise false.</returns>
        private static bool TryGetTypeNamesFromInterface(string interfaceFullTypeName, out string interfaceNamespace, out string controlTypeName)
        {
            int lastDotIndex = interfaceFullTypeName.LastIndexOf('.');
            if (lastDotIndex < 3)
            {
                interfaceNamespace = "";
                controlTypeName = "";
                return false;
            }

            controlTypeName = interfaceFullTypeName.Substring(lastDotIndex + 2);
            interfaceNamespace = interfaceFullTypeName.Substring(0, lastDotIndex);
            return true;
        }

        /// <summary>
        /// Roslyn calls this class when code changes are made. If we detect a change to the ImportStandardControlAttribute set
        /// we will generate a matching set of source files.
        /// </summary>
        class SyntaxReceiver : ISyntaxContextReceiver
        {
            public HashSet<string> InterfacesToGenerate = new HashSet<string>();

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                if (context.Node is AttributeSyntax attrib
                    && attrib.ArgumentList?.Arguments.Count == 1
                    && context.SemanticModel.GetTypeInfo(attrib).Type?.ToDisplayString() == "ImportStandardControlAttribute")
                {
                    string interfaceFullTypeName = context.SemanticModel.GetConstantValue(attrib.ArgumentList.Arguments[0].Expression).ToString();
                    InterfacesToGenerate.Add(interfaceFullTypeName);
                }
            }
        }
    }
}
