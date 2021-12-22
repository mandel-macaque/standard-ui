using System;
using System.Windows;

namespace Microsoft.StandardUI.Wpf
{
    /// <summary>
    /// This class is for UI controls passed in from the host, for native WPF controls, which
    /// we wrap with an IUIElement here.
    /// </summary>
    public class HostUIElement : IUIElement
    {
        private FrameworkElement _frameworkElement;

        public HostUIElement(FrameworkElement frameworkElement)
        {
            this._frameworkElement = frameworkElement;
        }

        void IUIElement.Measure(Size availableSize)
        {
            _frameworkElement.Measure(availableSize.ToWpfSize());
        }

        void IUIElement.Arrange(Rect finalRect)
        {
            _frameworkElement.Arrange(finalRect.ToWpfRect());
        }

        Size IUIElement.DesiredSize => _frameworkElement.DesiredSize.ToStandardUISize();

        double IUIElement.ActualX => throw new NotImplementedException();

        double IUIElement.ActualY => throw new NotImplementedException();

        HorizontalAlignment IUIElement.HorizontalAlignment
        {
            get => _frameworkElement.HorizontalAlignment.ToStandardUIHorizontalAlignment();
            set => _frameworkElement.HorizontalAlignment = value.ToWpfHorizontalAlignment();
        }

        VerticalAlignment IUIElement.VerticalAlignment
        {
            get => _frameworkElement.VerticalAlignment.FromWpfVerticalAlignment();
            set => _frameworkElement.VerticalAlignment = value.ToWpfVerticalAlignment();
        }

        FlowDirection IUIElement.FlowDirection
        {
            get => _frameworkElement.FlowDirection.ToStandardUIFlowDirection();
            set => _frameworkElement.FlowDirection = value.ToWpfFlowDirection();
        }

        // TODO: Error if appropriate when set to Visibility.Hidden
        bool IUIElement.IsVisible
        {
            get => _frameworkElement.Visibility != Visibility.Collapsed;
            set => _frameworkElement.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        double IUIElement.Width
        {
            get => _frameworkElement.Width;
            set => _frameworkElement.Width = value;
        }

        double IUIElement.MinWidth
        {
            get => _frameworkElement.MinWidth;
            set => _frameworkElement.MinWidth = value;
        }

        double IUIElement.MaxWidth
        {
            get => _frameworkElement.MaxWidth;
            set => _frameworkElement.MaxWidth = value;
        }

        double IUIElement.Height
        {
            get => _frameworkElement.Height;
            set => _frameworkElement.Height = value;
        }

        double IUIElement.MinHeight
        {
            get => _frameworkElement.MinHeight;
            set => _frameworkElement.MinHeight = value;
        }

        double IUIElement.MaxHeight
        {
            get => _frameworkElement.MaxHeight;
            set => _frameworkElement.MinHeight = value;
        }

        double IUIElement.ActualWidth => _frameworkElement.ActualWidth;

        double IUIElement.ActualHeight => _frameworkElement.ActualHeight;

        public object GetValue(IUIProperty property)
        {
            throw new NotImplementedException();
        }

        public object ReadLocalValue(IUIProperty property)
        {
            throw new NotImplementedException();
        }

        public void SetValue(IUIProperty property, object value)
        {
            throw new NotImplementedException();
        }
    }
}
