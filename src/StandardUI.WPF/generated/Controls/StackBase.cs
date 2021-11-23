// This file is generated from IStackBase.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class StackBase : Panel, IStackBase
    {
        public static readonly System.Windows.DependencyProperty SpacingProperty = PropertyUtils.Register(nameof(Spacing), typeof(double), typeof(StackBase), 0.0);
        
        public double Spacing
        {
            get => (double) GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }
    }
}
