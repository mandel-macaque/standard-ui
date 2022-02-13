// This file is generated from IGeometry.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.WinForms.Media
{
    public class Geometry : StandardUIObject, IGeometry
    {
        public static readonly UIProperty StandardFlatteningToleranceProperty = new UIProperty(nameof(StandardFlatteningTolerance), 0.25);
        public static readonly UIProperty TransformProperty = new UIProperty(nameof(Transform), null);
        
        public double StandardFlatteningTolerance
        {
            get => (double) GetValue(StandardFlatteningToleranceProperty);
            set => SetValue(StandardFlatteningToleranceProperty, value);
        }
        
        public Transform Transform
        {
            get => (Transform) GetValue(TransformProperty);
            set => SetValue(TransformProperty, value);
        }
        ITransform IGeometry.Transform
        {
            get => Transform;
            set => Transform = (Transform) value;
        }
    }
}
