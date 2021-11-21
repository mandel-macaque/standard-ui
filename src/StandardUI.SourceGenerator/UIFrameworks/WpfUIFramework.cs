namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public class WpfUIFramework : XamlUIFramework
    {
        public static readonly WpfUIFramework Instance = new WpfUIFramework();

        public override string ProjectBaseDirectory => "StandardUI.Wpf";
        public override string RootNamespace => "Microsoft.StandardUI.Wpf";
        public override string DependencyPropertyClassName => "System.Windows.DependencyProperty";
        public override string FrameworkTypeForUIElementAttachedTarget => "System.Windows.UIElement";
        public override string? DefaultBaseClassName => "StandardUIDependencyObject";
        public override string DefaultUIElementBaseClassName => "StandardUIFrameworkElement";
        public override string WrapperSuffix => "Wpf";

        public override void AddUsings(Usings usings, bool hasPropertyDescriptors, bool hasTypeConverterAttribute)
        {
            if (hasTypeConverterAttribute)
                usings.AddNamespace("System.ComponentModel");
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
}
