using Microsoft.AspNetCore.Components;
using Microsoft.StandardUI.DefaultImplementations;
using System;
using System.ComponentModel;

namespace Microsoft.StandardUI.Blazor
{
    public abstract class UIElement : ComponentBase, IUIElement
    {
        private PropertyValues _properties = new(true);

        public static readonly UIProperty WidthProperty = new(nameof(WidthProperty), double.NaN);
        public static readonly UIProperty MinWidthProperty = new(nameof(MinWidthProperty), 0.0);
        public static readonly UIProperty MaxWidthProperty = new(nameof(MaxWidthProperty), double.PositiveInfinity);

        public static readonly UIProperty HeightProperty = new(nameof(HeightProperty), double.NaN);
        public static readonly UIProperty MinHeightProperty = new(nameof(MinHeightProperty), 0.0);
        public static readonly UIProperty MaxHeightProperty = new(nameof(MaxHeightProperty), double.PositiveInfinity);

        public static readonly UIProperty MarginProperty = new(nameof(Margin), Thickness.Default);
        public static readonly UIProperty HorizontalAlignmentProperty = new(nameof(HorizontalAlignment), HorizontalAlignment.Stretch);
        public static readonly UIProperty VerticalAlignmentProperty = new(nameof(VerticalAlignment), VerticalAlignment.Stretch);

        public static readonly UIProperty FlowDirectionProperty = new(nameof(VerticalAlignment), FlowDirection.LeftToRight);
        public static readonly UIProperty VisibleProperty = new(nameof(Visible), true);


        public double Width
        {
            get => (double)GetNonNullValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }
        public double MinWidth
        {
            get => (double)GetNonNullValue(MinWidthProperty);
            set => SetValue(MinWidthProperty, value);
        }
        public double MaxWidth
        {
            get => (double)GetNonNullValue(MaxWidthProperty);
            set => SetValue(MaxWidthProperty, value);
        }

        public double Height
        {
            get => (double)GetNonNullValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }
        public double MinHeight
        {
            get => (double)GetNonNullValue(MinHeightProperty);
            set => SetValue(MinHeightProperty, value);
        }
        public double MaxHeight
        {
            get => (double)GetNonNullValue(MaxHeightProperty);
            set => SetValue(MaxHeightProperty, value);
        }

        public Thickness Margin
        {
            get => (Thickness)GetNonNullValue(MarginProperty);
            set => SetValue(MarginProperty, value);
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get => (HorizontalAlignment)GetNonNullValue(HorizontalAlignmentProperty);
            set => SetValue(HorizontalAlignmentProperty, value);
        }
        public VerticalAlignment VerticalAlignment
        {
            get => (VerticalAlignment)GetNonNullValue(VerticalAlignmentProperty);
            set => SetValue(VerticalAlignmentProperty, value);
        }

        public FlowDirection FlowDirection
        {
            get => (FlowDirection)GetNonNullValue(FlowDirectionProperty);
            set => SetValue(FlowDirectionProperty, value);
        }
        public bool Visible
        {
            get => (bool)GetNonNullValue(VisibleProperty);
            set => SetValue(VisibleProperty, value);
        }

        public abstract void Measure(Size availableSize);

        public abstract void Arrange(Rect finalRect);

        public Size DesiredSize => throw new NotImplementedException();

        public double ActualX => throw new NotImplementedException();

        public double ActualY => throw new NotImplementedException();

        public double ActualWidth => throw new NotImplementedException();

        public double ActualHeight => throw new NotImplementedException();

        public object GetNonNullValue(UIProperty property) => _properties.GetNonNullValue(property);
        public object? GetValue(UIProperty property) => _properties.GetValue(property);
        object? IUIObject.GetValue(IUIProperty property) => _properties.GetValue((UIProperty)property);

        public object? ReadLocalValue(UIProperty property) => _properties.ReadLocalValue(property);
        object? IUIObject.ReadLocalValue(IUIProperty property) => _properties.ReadLocalValue((UIProperty)property);

        public void SetValue(UIProperty property, object? value) => _properties.SetValue(property, value);
        void IUIObject.SetValue(IUIProperty property, object? value) => _properties.SetValue((UIProperty)property, value);

        public void ClearValue(UIProperty property) => _properties.ClearValue(property);
        void IUIObject.ClearValue(IUIProperty property) => _properties.ClearValue((UIProperty)property);
    }
}
