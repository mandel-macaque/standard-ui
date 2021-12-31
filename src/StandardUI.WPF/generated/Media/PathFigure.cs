// This file is generated from IPathFigure.cs. Update the source file to change its contents.

using System.Collections.Generic;
using Microsoft.StandardUI.Media;
using DependencyProperty = System.Windows.DependencyProperty;

namespace Microsoft.StandardUI.Wpf.Media
{
    public class PathFigure : BuiltInDependencyObject, IPathFigure
    {
        public static readonly DependencyProperty SegmentsProperty = PropertyUtils.Register(nameof(Segments), typeof(IEnumerable<IPathSegment>), typeof(PathFigure), null);
        public static readonly DependencyProperty StartPointProperty = PropertyUtils.Register(nameof(StartPoint), typeof(PointWpf), typeof(PathFigure), PointWpf.Default);
        public static readonly DependencyProperty IsClosedProperty = PropertyUtils.Register(nameof(IsClosed), typeof(bool), typeof(PathFigure), false);
        public static readonly DependencyProperty IsFilledProperty = PropertyUtils.Register(nameof(IsFilled), typeof(bool), typeof(PathFigure), true);
        
        public IEnumerable<IPathSegment> Segments
        {
            get => (IEnumerable<IPathSegment>) GetValue(SegmentsProperty);
            set => SetValue(SegmentsProperty, value);
        }
        
        public PointWpf StartPoint
        {
            get => (PointWpf) GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }
        Point IPathFigure.StartPoint
        {
            get => StartPoint.Point;
            set => StartPoint = new PointWpf(value);
        }
        
        public bool IsClosed
        {
            get => (bool) GetValue(IsClosedProperty);
            set => SetValue(IsClosedProperty, value);
        }
        
        public bool IsFilled
        {
            get => (bool) GetValue(IsFilledProperty);
            set => SetValue(IsFilledProperty, value);
        }
    }
}
