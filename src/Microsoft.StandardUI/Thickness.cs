namespace Microsoft.StandardUI
{
    /// <summary>
    /// Describes the thickness of a frame around a rectangle. Four Double values describe the Left, Top,
    /// Right, and Bottom sides of the rectangle, respectively.
    /// </summary>
    public struct Thickness
    {
        public static readonly Thickness Default = new Thickness();

        /// <summary>
        /// The left side measure of the Thickness.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// The top edge measure of the Thickness.
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// The right side measure of the Thickness.
        /// </summary>
        public double Right { get; set; }

        /// <summary>
        /// The bottom edge measure of the Thickness.
        /// </summary>
        public double Bottom { get; set; }

        public Thickness(double uniformLength) : this(uniformLength, uniformLength, uniformLength, uniformLength)
        {
        }

        public Thickness(double left, double top, double right, double bottom) : this()
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public double HorizontalThickness => Left + Right;

        public double VerticalThickness => Top + Bottom;
    }
}
