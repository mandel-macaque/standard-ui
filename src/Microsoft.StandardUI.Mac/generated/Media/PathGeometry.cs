// This file is generated from IPathGeometry.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.Mac.Media
{
    public class PathGeometry : Geometry, IPathGeometry
    {
        public static readonly UIProperty FiguresProperty = new UIProperty(nameof(Figures), null, readOnly:true);
        public static readonly UIProperty FillRuleProperty = new UIProperty(nameof(FillRule), FillRule.EvenOdd);
        
        private UICollection<IPathFigure> _figures;
        
        public PathGeometry()
        {
            _figures = new UICollection<IPathFigure>(this);
            SetValue(FiguresProperty, _figures);
        }
        
        public UICollection<IPathFigure> Figures => _figures;
        IUICollection<IPathFigure> IPathGeometry.Figures => Figures;
        
        public FillRule FillRule
        {
            get => (FillRule) GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }
    }
}
