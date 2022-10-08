using System;
using System.Collections.Generic;
using Microsoft.StandardUI.Blazor.Controls;
using Microsoft.StandardUI.Blazor.Media;
using Microsoft.StandardUI.Blazor.Shapes;
using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;

namespace Microsoft.StandardUI.Blazor
{
    public class StandardUIFactory : IStandardUIFactory
    {
        /*** UI Elements ***/

        public ICanvas CreateCanvas() => new Canvas();
        public ICanvasAttached CanvasAttachedInstance => CanvasAttached.Instance;
        public IStackPanel CreateStackPanel() => new StackPanel();
        public IVerticalStack CreateVerticalStack() => new VerticalStack();
        public IHorizontalStack CreateHorizontalStack() => new HorizontalStack();
        public IGrid CreateGrid() => new Grid();
        public IGridAttached GridAttachedInstance => GridAttached.Instance;
        public IRowDefinition CreateRowDefinition() => new RowDefinition();
        public IColumnDefinition CreateColumnDefinition() => new ColumnDefinition();
        public IEllipse CreateEllipse() => new Ellipse();
        public ILine CreateLine() => new Line();
        public IPath CreatePath() => new Path();
        public IPolygon CreatePolygon() => new Polygon();
        public IPolyline CreatePolyline() => new Polyline();
        public IRectangle CreateRectangle() => new Rectangle();

        public ITextBlock CreateTextBlock() => new TextBlock();

        /*** Media objects ***/

        public ISolidColorBrush CreateSolidColorBrush() => new SolidColorBrush();
        public ILinearGradientBrush CreateLinearGradientBrush() => new LinearGradientBrush();
        public IRadialGradientBrush CreateRadialGradientBrush() => new RadialGradientBrush();

        public ILineSegment CreateLineSegment() => new LineSegment();
        public IPolyLineSegment CreatePolyLineSegment() => new PolyLineSegment();
        public IBezierSegment CreateBezierSegment() => new BezierSegment();
        public IPolyBezierSegment CreatePolyBezierSegment() => new PolyBezierSegment();
        public IQuadraticBezierSegment CreateQuadraticBezierSegment() => new QuadraticBezierSegment();
        public IPolyQuadraticBezierSegment CreatePolyQuadraticBezierSegment() => new PolyQuadraticBezierSegment();
        public IArcSegment CreateArcSegment() => new ArcSegment();
        public IPathGeometry CreatePathGeometry() => new PathGeometry();
        public IPathFigure CreatePathFigure() => new PathFigure();
    }
}
