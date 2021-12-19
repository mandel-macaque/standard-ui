using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.WinUI.NativeVisualEnvironment;
using System;

namespace Microsoft.StandardUI.WinUI
{
    public class StandardControl : Microsoft.UI.Xaml.Controls.Control, IStandardControl, IStandardControlEnvironmentPeer
    {
        private StandardControlImplementation _implementation;
        private StandardUIFrameworkElement? _buildContent;
        private bool _invalid = true;

        public StandardControl()
        {
            if (!StandardUIEnvironment.IsInitialized)
            {
                WinUIStandardUIEnvironment.Init(new WinUINativeVisualEnvironment());
            }
        }

        protected void InitImplementation(StandardControlImplementation implementation)
        {
            _implementation = implementation;
        }

        void IUIElement.Measure(Size availableSize)
        {
            Measure(availableSize.ToWindowsFoundationSize());
        }

        void IUIElement.Arrange(Rect finalRect)
        {
            Arrange(finalRect.ToWindowsFoundationRect());
        }

        IUIPropertyObject? IStandardControl.GetTemplateChild(string childName)
        {
            throw new NotImplementedException();
        }

        protected override global::Windows.Foundation.Size MeasureOverride(global::Windows.Foundation.Size constraint)
        {
            if (_invalid)
            {
                Rebuild();
                _invalid = false;
            }

            _implementation.Measure(new Size(constraint.Width, constraint.Height));
            return _implementation.DesiredSize.ToWindowsFoundationSize();
        }

        protected override global::Windows.Foundation.Size ArrangeOverride(global::Windows.Foundation.Size arrangeSize)
        {
            _implementation.Arrange(new Rect(0, 0, arrangeSize.Width, arrangeSize.Height));
            return arrangeSize;
        }

        Size IUIElement.DesiredSize => SizeExtensions.FromWindowsFoundationSize(DesiredSize);

        double IUIElement.ActualX => throw new NotImplementedException();

        double IUIElement.ActualY => throw new NotImplementedException();

        HorizontalAlignment IUIElement.HorizontalAlignment
        {
            get => HorizontalAlignmentExtensions.FromWinUIHorizontalAlignment(this.HorizontalAlignment);
            set => this.HorizontalAlignment = value.ToWinUIHorizontalAlignment();
        }

        VerticalAlignment IUIElement.VerticalAlignment
        {
            get => VerticalAlignmentExtensions.FromWinUIVerticalAlignment(this.VerticalAlignment);
            set => this.VerticalAlignment = value.ToWinUIVerticalAlignment();
        }

        FlowDirection IUIElement.FlowDirection
        {
            get => FlowDirectionExtensions.FromWinUIFlowDirection(this.FlowDirection);
            set => this.FlowDirection = value.ToWinUIFlowDirection();
        }

        // TODO: Error if appropriate when set to Visibility.Hidden
        bool IUIElement.IsVisible
        {
            get => this.Visibility != Microsoft.UI.Xaml.Visibility.Collapsed;
            set => this.Visibility = value ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
        }

        public object GetValue(IUIProperty dp)
        {
            Microsoft.UI.Xaml.DependencyProperty wpfDependencyProperty = ((UIProperty)dp).DependencyProperty;
            return GetValue(wpfDependencyProperty);
        }

        public object ReadLocalValue(IUIProperty dp)
        {
            Microsoft.UI.Xaml.DependencyProperty wpfDependencyProperty = ((UIProperty)dp).DependencyProperty;
            return ReadLocalValue(wpfDependencyProperty);
        }

        public void SetValue(IUIProperty dp, object value)
        {
            Microsoft.UI.Xaml.DependencyProperty wpfDependencyProperty = ((UIProperty)dp).DependencyProperty;
            SetValue(wpfDependencyProperty, value);
        }

        IUIElement? IStandardControlEnvironmentPeer.BuildContent => _buildContent;

#if LATER
        protected override int VisualChildrenCount => _buildContent != null ? 1 : 0;

        protected override Visual GetVisualChild(int index)
        {
            if (_buildContent == null)
                throw new ArgumentOutOfRangeException("index", index, "Control returned null from build");
            if (index != 0)
                throw new ArgumentOutOfRangeException("index", index, "Index out of range; control only has a single visual child.");

            return _buildContent;
        }
#endif

        private void Rebuild()
        {
#if LATER
            if (_buildContent != null)
            {
                RemoveVisualChild(_buildContent);
                RemoveLogicalChild(_buildContent);
                _buildContent = null;
            }

            _buildContent = (StandardUIFrameworkElement?)_implementation.Build();

            if (_buildContent != null)
            {
                AddVisualChild(_buildContent);
                AddLogicalChild(_buildContent);
            }
#endif
        }

#if false
        IControlTemplate? IControl.Template
        {
            get
            {
                System.Windows.Controls.ControlTemplate controlTemplate = Template;
                if (controlTemplate == null)
                    _controlTemplateWpf = null;
                else
                {
                    // Cache our ControlTemplateWpf wrapper, so the reference stays the same except
                    // when the underlying Template changes
                    if (!(ReferenceEquals(_controlTemplateWpf?.ControlTemplate, controlTemplate)))
                        _controlTemplateWpf = new ControlTemplateWpf(controlTemplate);
                }
                return _controlTemplateWpf;
            }

            set
            {
                var controlTemplateWpf = (ControlTemplateWpf?)value;
                Template = controlTemplateWpf?.ControlTemplate;
                _controlTemplateWpf = controlTemplateWpf;
            }
        }
#endif

#if false
        IUIPropertyObject? IStandardUIControlEnvironmentPeer.GetTemplateChild(string childName)
        {
            Microsoft.UI.Xaml.DependencyObject? child = this.GetTemplateChild(childName);
            if (child == null)
                return null;

            // TODO:Finish this
            return null;
        }
#endif
    }
}
