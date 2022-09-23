using System;
using Microsoft.StandardUI.Blazor.NativeVisualFramework;
using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Blazor
{
    public partial class StandardControl : UIElement, IStandardControl, IStandardControlEnvironmentPeer
    {
        private StandardControlImplementation? _implementation;
        private IUIElement? _buildContent;
        private bool _invalid = true;

        public StandardControl()
        {
            if (!HostEnvironment.IsInitialized)
                BlazorHostFramework.Init(new BlazorNativeVisualFramework());
        }

        protected void InitImplementation(StandardControlImplementation implementation)
        {
            _implementation = implementation;
        }

        IUIObject? IStandardControl.GetTemplateChild(string childName) => throw new NotImplementedException();

        public override void Measure(Size constraint) => throw new NotImplementedException();

        public override void Arrange(Rect finalRect) => throw new NotImplementedException();

        IUIElement? IStandardControlEnvironmentPeer.BuildContent => _buildContent;

        private void Rebuild()
        {
#if LATER
            if (_buildContent != null)
            {
                System.Windows.UIElement wpfUIElement =_buildContent.ToWpfUIElement();
                RemoveVisualChild(wpfUIElement);
                RemoveLogicalChild(wpfUIElement);
                _buildContent = null;
            }

            _buildContent = _implementation!.Build();

            if (_buildContent != null)
            {
                System.Windows.UIElement wpfUIElement = _buildContent.ToWpfUIElement();
                AddVisualChild(wpfUIElement);
                AddLogicalChild(wpfUIElement);
            }
#endif
        }
    }
}
