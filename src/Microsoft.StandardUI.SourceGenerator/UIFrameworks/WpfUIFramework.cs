namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public class WpfUIFramework : XamlUIFramework
    {
        public WpfUIFramework(Context context) : base(context)
        {
        }

        public override string ProjectBaseDirectory => "Microsoft.StandardUI.Wpf";
        public override string RootNamespace => "Microsoft.StandardUI.Wpf";
        public override TypeName DependencyPropertyType => new("System.Windows", "DependencyProperty");
        public override TypeName ContentPropertyAttribute => new("System.Windows.Markup", "ContentPropertyAttribute");

        public override string FrameworkTypeForUIElementAttachedTarget => "System.Windows.UIElement";
        public override string ToFrameworkTypeForUIElementAttachedTarget => "ToWpfUIElement";

        public override string NativeUIElementType => "System.Windows.FrameworkElement";
        public override string WrapperSuffix => "Wpf";
        protected override string FontFamilyDefaultValue => "FontFamilyExtensions.DefaultFontFamily";

        public override void GenerateStandardPanelLayoutMethods(string layoutManagerTypeName, Source methods)
        {
            methods.AddBlankLineIfNonempty();
            methods.AddLine($"protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) =>");
            using (methods.Indent())
            {
                methods.AddLine(
                    $"{layoutManagerTypeName}.Instance.MeasureOverride(this, constraint.ToStandardUISize()).ToWpfSize();");
            }

            methods.AddBlankLine();
            methods.AddLine($"protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) =>");
            using (methods.Indent())
            {
                methods.AddLine(
                    $"{layoutManagerTypeName}.Instance.ArrangeOverride(this, arrangeSize.ToStandardUISize()).ToWpfSize();");
            }
        }

        public override void GeneratePanelMethods(Source methods)
        {
            methods.AddBlankLineIfNonempty();

            methods.AddLine(
                "protected override int VisualChildrenCount => _children.Count;");
            methods.AddBlankLine();
            methods.AddLine(
                "protected override System.Windows.Media.Visual GetVisualChild(int index) => (System.Windows.Media.Visual) _children[index];");
        }

        public override void GenerateDrawableObjectMethods(Interface intface, Source methods)
        {
            methods.AddBlankLineIfNonempty();
            methods.AddLine(
                $"public override void Draw(IDrawingContext drawingContext) => drawingContext.Draw{intface.FrameworkClassName}(this);");

            if (intface.IsThisType(KnownTypes.ITextBlock))
            {
                methods.AddLine(
                    $"protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) =>");
                using (methods.Indent())
                {
                    methods.AddLine(
                        $"HostEnvironment.VisualFramework.MeasureTextBlock(this).ToWpfSize();");
                }
            }
        }

        public override void GenerateBuiltInIUIElementPartialClasses()
        {
            GenerateBuiltInIUIElementPartialClass("BuiltInUIElement", "FrameworkElement, IUIElement, ILogicalParent");
            GenerateBuiltInIUIElementPartialClass("StandardControl", "System.Windows.Controls.Control, IStandardControl, IStandardControlEnvironmentPeer, ILogicalParent", "");
        }

        private void GenerateBuiltInIUIElementPartialClass(string className, string derviedFrom, params string[] extraUsings)
        {
            var classSource = new ClassSource(Context,
                namespaceName: RootNamespace,
                isPartial: true,
                className: className,
                derivedFrom: derviedFrom,
                fileNameOverride: className + ".UIElement");

            classSource.Usings.AddNamespace("System");
            classSource.Usings.AddNamespace("System.Windows");
            foreach (string extraUsing in extraUsings)
                classSource.Usings.AddNamespace(extraUsing);

            GenerateIUIElementMethods(classSource);
            classSource.AddToOutput(this);
        }

        public override void GenerateIUIElementMethods(ClassSource classSource)
        {
            Source methods = classSource.NonstaticMethods;

            // TODO: Error if appropriate when set to Visibility.Hidden

            methods.AddLines(
                "void IUIElement.Measure(Size availableSize) => Measure(availableSize.ToWpfSize());",
                "void IUIElement.Arrange(Rect finalRect) => Arrange(finalRect.ToWpfRect());",
                "Size IUIElement.DesiredSize => DesiredSize.ToStandardUISize();",
                "",
                "double IUIElement.ActualX => throw new NotImplementedException();",
                "double IUIElement.ActualY => throw new NotImplementedException();",
                "");
            methods.AddProperty("Thickness IUIElement.Margin", "Margin.ToStandardUIThickness()", "Margin = value.ToWpfThickness()");
            methods.AddBlankLine();
            methods.AddProperty("HorizontalAlignment IUIElement.HorizontalAlignment", "HorizontalAlignment.ToStandardUIHorizontalAlignment()", "HorizontalAlignment = value.ToWpfHorizontalAlignment()");
            methods.AddBlankLine();
            methods.AddProperty("VerticalAlignment IUIElement.VerticalAlignment", "VerticalAlignment.ToStandardUIVerticalAlignment()", "VerticalAlignment = value.ToWpfVerticalAlignment()");
            methods.AddBlankLine();
            methods.AddProperty("FlowDirection IUIElement.FlowDirection", "FlowDirection.ToStandardUIFlowDirection()", "FlowDirection = value.ToWpfFlowDirection()");
            methods.AddBlankLine();
            methods.AddProperty("bool IUIElement.Visible", "Visibility != Visibility.Collapsed", "Visibility = value ? Visibility.Visible : Visibility.Collapsed");
            methods.AddBlankLine();
            methods.AddProperty("double IUIElement.Width", "Width", "Width = value");
            methods.AddBlankLine();
            methods.AddProperty("double IUIElement.MinWidth", "MinWidth", "MinWidth = value");
            methods.AddBlankLine();
            methods.AddProperty("double IUIElement.MaxWidth", "MaxWidth", "MaxWidth = value");
            methods.AddBlankLine();
            methods.AddProperty("double IUIElement.Height", "Height", "Height = value");
            methods.AddBlankLine();
            methods.AddProperty("double IUIElement.MinHeight", "MinHeight", "MinHeight = value");
            methods.AddBlankLine();
            methods.AddProperty("double IUIElement.MaxHeight", "MaxHeight", "MaxHeight = value");
            methods.AddBlankLine();
            methods.AddLines(
                "",
                "double IUIElement.ActualWidth => ActualWidth;",
                "double IUIElement.ActualHeight => ActualHeight;",
                "",
                "object? IUIObject.GetValue(IUIProperty property) => GetValue(((UIProperty)property).DependencyProperty);",
                "object? IUIObject.ReadLocalValue(IUIProperty property) => ReadLocalValue(((UIProperty)property).DependencyProperty);",
                "void IUIObject.SetValue(IUIProperty property, object? value) => SetValue(((UIProperty)property).DependencyProperty, value);",
                "void IUIObject.ClearValue(IUIProperty property) => ClearValue(((UIProperty)property).DependencyProperty);");
        }
    }
}
