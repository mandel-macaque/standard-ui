namespace Microsoft.StandardUI.Controls
{
    public class VerticalStackLayoutManager : StackBaseLayoutManager<IVerticalStack>
    {
        public static VerticalStackLayoutManager Instance = new VerticalStackLayoutManager();

        public override Size MeasureOverride(IVerticalStack stack, Size constraint)
        {
            return MeasureOverrideVertical(stack, constraint);
        }

        public override Size ArrangeOverride(IVerticalStack stack, Size finalSize)
        {
            return ArrangeOverrideVertical(stack, finalSize);
        }
    }
}
