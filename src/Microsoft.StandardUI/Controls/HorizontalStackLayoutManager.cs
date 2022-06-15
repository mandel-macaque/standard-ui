namespace Microsoft.StandardUI.Controls
{
    public class HorizontalStackLayoutManager : StackBaseLayoutManager<IHorizontalStack>
    {
        public static HorizontalStackLayoutManager Instance = new HorizontalStackLayoutManager();

        public override Size MeasureOverride(IHorizontalStack stack, Size constraint)
        {
            return MeasureOverrideHorizontal(stack, constraint);
        }

		public override Size ArrangeOverride(IHorizontalStack stack, Size finalSize)
		{
			return ArrangeOverrideHorizontal(stack, finalSize);
		}
	}
}
