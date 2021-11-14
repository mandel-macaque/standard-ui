using System.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.StandardUI.SourceGenerator
{
    public abstract class FrameworkType
    {
        public abstract string ProjectBaseDirectory { get; }
        public abstract string RootNamespace { get; }
        public abstract string FrameworkTypeForUIElementAttachedTarget { get; }
        public abstract string? DefaultBaseClassName { get; }
        public abstract string DefaultUIElementBaseClassName { get; }
        public virtual void AddUsings(Usings usings, bool hasPropertyDescriptors, bool hasTypeConverterAttribute) { }
        public virtual void AddTypeAliasUsingIfNeeded(Usings usings, string destinationTypeName) { }
        public virtual void GenerateStandardPanelLayoutMethods(Source methodsSource, string layoutManagerTypeName) { }
        public abstract bool EmitChangedNotifications { get; }
    }

    public abstract class XamlFrameworkType : FrameworkType
    {
        public abstract string DependencyPropertyClassName { get; }
        public virtual string GetPropertyDescriptorName(string propertyName) => propertyName + "Property";
        public override bool EmitChangedNotifications => true;
        public abstract string WrapperSuffix { get; }
        public virtual void GeneratePanelSubclassMethods(Source methods) { }
    }

    public class WpfFrameworkType : XamlFrameworkType
    {
        public static readonly WpfFrameworkType Instance = new WpfFrameworkType();
        public static QualifiedNameSyntax SystemWindows = QualifiedName(IdentifierName("System"), IdentifierName("Windows"));

        public override string ProjectBaseDirectory => "StandardUI.Wpf";
        public override string RootNamespace => "Microsoft.StandardUI.Wpf";
        public override string DependencyPropertyClassName => "System.Windows.DependencyProperty";
        public override string FrameworkTypeForUIElementAttachedTarget => "System.Windows.UIElement";
        public override string? DefaultBaseClassName => "StandardUIDependencyObject";
        public override string DefaultUIElementBaseClassName => "StandardUIFrameworkElement";
        public override string WrapperSuffix => "Wpf";

        public override void AddUsings(Usings usings, bool hasPropertyDescriptors, bool hasTypeConverterAttribute)
        {
#if NOT_NEEDED
            if (hasPropertyDescriptors)
                usings.Add(QualifiedName(IdentifierName("System"), IdentifierName("Windows")));
#endif
            if (hasTypeConverterAttribute)
                usings.AddNamespace("System.ComponentModel");

#if NOT_NEEDED
            usings.Add(QualifiedName(
                    QualifiedName(IdentifierName("System"), IdentifierName("Windows")),
                    IdentifierName("Markup")));
#endif
        }

        public override void GenerateStandardPanelLayoutMethods(Source source, string layoutManagerTypeName)
        {
            if (!source.IsEmpty)
                source.AddBlankLine();
            source.AddLine($"protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) =>");
            using (source.Indent())
            {
                source.AddLine(
                    $"{layoutManagerTypeName}.Instance.MeasureOverride(this, SizeExtensions.FromWpfSize(constraint)).ToWpfSize();");
            }

            source.AddBlankLine();
            source.AddLine($"protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) =>");
            using (source.Indent())
            {
                source.AddLine(
                    $"{layoutManagerTypeName}.Instance.MeasureOverride(this, SizeExtensions.FromWpfSize(arrangeSize)).ToWpfSize();");
            }
        }
    }

    public class UwpFrameworkType : XamlFrameworkType
    {
        public static readonly UwpFrameworkType Instance = new UwpFrameworkType();

        public override string ProjectBaseDirectory => "StandardUI.UWP";
        public override string RootNamespace => "Microsoft.StandardUI.Uwp";
        public override string DependencyPropertyClassName => "DependencyProperty";
        public override string FrameworkTypeForUIElementAttachedTarget => "UIElement";
        public override string? DefaultBaseClassName => "StandardUIDependencyObject";
        public override string DefaultUIElementBaseClassName => "StandardUIUIElement";
        public override string WrapperSuffix => "Uwp";
    }

    public class XamarinFormsFrameworkType : XamlFrameworkType
    {
        public static readonly XamarinFormsFrameworkType Instance = new XamarinFormsFrameworkType();

        public override string ProjectBaseDirectory => "StandardUI.XamarinForms";
        public override string RootNamespace => "Microsoft.StandardUI.XamarinForms";
        public override string DependencyPropertyClassName => "BindableProperty";
        public override string FrameworkTypeForUIElementAttachedTarget => "VisualElement";
        public override string? DefaultBaseClassName => "StandardUIBindableObject";
        public override string DefaultUIElementBaseClassName => "StandardUIView";
        public override string WrapperSuffix => "XamarinForms";

        public override void AddUsings(Usings usings, bool hasPropertyDescriptors, bool hasTypeConverterAttribute)
        {
            usings.AddNamespace("Xamarin.Forms");
        }

        public override void AddTypeAliasUsingIfNeeded(Usings usings, string destinationTypeName)
        {
            // These types are also defined in Xamarin.Forms, so add aliases to prefer the Standard UI type
            if (destinationTypeName == "Brush" || destinationTypeName == "Brush?")
                usings.AddTypeAlias("Brush = Microsoft.StandardUI.XamarinForms.Media.Brush");
            else if (destinationTypeName == "SweepDirection")
                usings.AddTypeAlias("SweepDirection = Microsoft.StandardUI.Media.SweepDirection");
        }
    }

    public class StandardModelFrameworkType : FrameworkType
    {
        public static readonly StandardModelFrameworkType Instance = new StandardModelFrameworkType();

        public override string ProjectBaseDirectory => Path.Combine("StandardUI", "StandardModel");
        public override string RootNamespace => "Microsoft.StandardUI.StandardModel";
        public override string FrameworkTypeForUIElementAttachedTarget => "ObjectWithCascadingNotifications";
        public override string? DefaultBaseClassName => "ObjectWithCascadingNotifications";
        public override string DefaultUIElementBaseClassName => "ObjectWithCascadingNotifications";

        public override bool EmitChangedNotifications => false;
    }
}
