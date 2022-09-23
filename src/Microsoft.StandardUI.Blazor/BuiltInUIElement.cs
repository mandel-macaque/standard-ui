namespace Microsoft.StandardUI.Blazor
{
    /// <summary>
    /// This is the base for predefined Standard UI controls.
    /// </summary>
    public partial class BuiltInUIElement : UIElement
    {
        public virtual void Draw(IDrawingContext visualizer)
        {
        }

        public override void Measure(Size availableSize) => throw new System.NotImplementedException();

        public override void Arrange(Rect finalRect) => throw new System.NotImplementedException();
    }
}
