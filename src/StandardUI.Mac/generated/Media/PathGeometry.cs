// This file is generated from IPathGeometry.cs. Update the source file to change its contents.

using System.Collections.Generic;
using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.Mac.Media
{
    public class PathGeometry : Geometry, IPathGeometry
    {
        public static readonly UIProperty FiguresProperty = new UIProperty(nameof(Figures), null);
        public static readonly UIProperty FillRuleProperty = new UIProperty(nameof(FillRule), FillRule.EvenOdd);
        
        public IEnumerable<IPathFigure> Figures
        {
            get => (IEnumerable<IPathFigure>) GetValue(FiguresProperty);
            set => SetValue(FiguresProperty, value);
        }
        
        public FillRule FillRule
        {
            get => (FillRule) GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }
    }
}
