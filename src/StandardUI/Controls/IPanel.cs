namespace Microsoft.StandardUI.Controls
{
    [UIModelObject]
    public interface IPanel : IUIElement
    {
        IUICollection<IUIElement> Children { get; }
    }
}
