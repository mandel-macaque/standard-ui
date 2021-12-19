namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public class WinUIUIFramework : XamlUIFramework
    {
        public WinUIUIFramework(Context context) : base(context)
        {
        }

        public override string ProjectBaseDirectory => "StandardUI.WinUI";
        public override string RootNamespace => "Microsoft.StandardUI.WinUI";
        public override string? DependencyPropertyTypeAlias => "DependencyProperty = Microsoft.UI.Xaml.DependencyProperty";
        public override string DependencyPropertyClassName => "DependencyProperty";
        public override string FrameworkTypeForUIElementAttachedTarget => "Microsoft.UI.Xaml.FrameworkElement";
        public override string? DefaultBaseClassName => "StandardUIDependencyObject";
        public override string DefaultUIElementBaseClassName => "StandardUIFrameworkElement";
        public override string WrapperSuffix => "WinUI";
        protected override string FontFamilyDefaultValue => "FontFamilyExtensions.DefaultFontFamily";

        public override void AddUsings(Usings usings, bool hasPropertyDescriptors, bool hasTypeConverterAttribute)
        {
            if (hasTypeConverterAttribute)
                usings.AddNamespace("System.ComponentModel");
        }

        public override void GenerateStandardPanelLayoutMethods(string layoutManagerTypeName, Source methods)
        {
            methods.AddBlankLineIfNonempty();
            methods.AddLine($"protected override global::Windows.Foundation.Size MeasureOverride(global::Windows.Foundation.Size constraint) =>");
            using (methods.Indent())
            {
                methods.AddLine(
                    $"{layoutManagerTypeName}.Instance.MeasureOverride(this, SizeExtensions.FromWindowsFoundationSize(constraint)).ToWindowsFoundationSize();");
            }

            methods.AddBlankLine();
            methods.AddLine($"protected override global::Windows.Foundation.Size ArrangeOverride(global::Windows.Foundation.Size arrangeSize) =>");
            using (methods.Indent())
            {
                methods.AddLine(
                    $"{layoutManagerTypeName}.Instance.ArrangeOverride(this, SizeExtensions.FromWindowsFoundationSize(arrangeSize)).ToWindowsFoundationSize();");
            }
        }

        public override void GeneratePanelMethods(Source methods)
        {
#if LATER
            methods.AddBlankLineIfNonempty();

            methods.AddLine(
                "protected override int VisualChildrenCount => _children.Count;");
            methods.AddBlankLine();
            methods.AddLine(
                "protected override System.Windows.Media.Visual GetVisualChild(int index) => (System.Windows.Media.Visual) _children[index];");
#endif
        }

        public override void GenerateDrawableObjectMethods(Interface intface, Source methods)
        {
            methods.AddBlankLineIfNonempty();
            methods.AddLine(
                $"public override void Draw(IDrawingContext drawingContext) => drawingContext.Draw{intface.FrameworkClassName}(this);");

            if (intface.IsThisType(KnownTypes.ITextBlock))
            {
                methods.AddLine(
                    $"protected override global::Windows.Foundation.Size MeasureOverride(global::Windows.Foundation.Size constraint) =>");
                using (methods.Indent())
                {
                    methods.AddLine(
                        $"StandardUIEnvironment.Instance.VisualEnvironment.MeasureTextBlock(this).ToWindowsFoundationSize();");
                }
            }
        }
    }
}
