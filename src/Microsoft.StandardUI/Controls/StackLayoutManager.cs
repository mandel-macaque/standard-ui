﻿namespace Microsoft.StandardUI.Controls
{
    public class StackLayoutManager : StackBaseLayoutManager<IStack>
    {
        public static StackLayoutManager Instance = new StackLayoutManager();

        public override Size MeasureOverride(IStack stack, Size constraint)
        {
            return stack.Orientation == Orientation.Horizontal ?
                MeasureOverrideHorizontal(stack, constraint) :
                MeasureOverrideVertical(stack, constraint);
        }

        public override Size ArrangeOverride(IStack stack, Size finalSize)
        {
            return stack.Orientation == Orientation.Horizontal ?
                ArrangeOverrideHorizontal(stack, finalSize) :
                ArrangeOverrideVertical(stack, finalSize);
        }
    }
}
