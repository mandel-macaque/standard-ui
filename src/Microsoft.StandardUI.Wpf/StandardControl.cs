using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Wpf.NativeVisualFramework;
using System;
using System.Windows.Media;

namespace Microsoft.StandardUI.Wpf
{
    public partial class StandardControl : System.Windows.Controls.Control, IStandardControl, IStandardControlEnvironmentPeer, ILogicalParent
    {
        private StandardControlImplementation? _implementation;
        private IUIElement? _buildContent;
        private bool _invalid = true;

        public StandardControl()
        {
            if (!HostEnvironment.IsInitialized)
            {
                WpfHostFramework.Init(new WpfNativeVisualFramework());
            }
        }

        protected void InitImplementation(StandardControlImplementation implementation)
        {
            _implementation = implementation;
        }

        IUIObject? IStandardControl.GetTemplateChild(string childName)
        {
            throw new NotImplementedException();
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint)
        {
            if (_invalid)
            {
                Rebuild();
                _invalid = false;
            }

            _implementation!.Measure(new Size(constraint.Width, constraint.Height));
            return _implementation.DesiredSize.ToWpfSize();
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize)
        {
            _implementation!.Arrange(new Rect(0, 0, arrangeSize.Width, arrangeSize.Height));
            return arrangeSize;
        }

        protected override int VisualChildrenCount => _buildContent != null ? 1 : 0;

        IUIElement? IStandardControlEnvironmentPeer.BuildContent => _buildContent;

        protected override Visual GetVisualChild(int index)
        {
            if (_buildContent == null)
                throw new ArgumentOutOfRangeException("index", index, "Control returned null from build");
            if (index != 0)
                throw new ArgumentOutOfRangeException("index", index, "Index out of range; control only has a single visual child.");

            return _buildContent.ToWpfUIElement();
        }

        void ILogicalParent.AddLogicalChild(object child) => this.AddLogicalChild(child);

        void ILogicalParent.RemoveLogicalChild(object child) => this.RemoveLogicalChild(child);

        private void Rebuild()
        {
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
            System.Windows.DependencyObject? child = this.GetTemplateChild(childName);
            if (child == null)
                return null;

            // TODO:Finish this
            return null;
        }
#endif
    }
}
