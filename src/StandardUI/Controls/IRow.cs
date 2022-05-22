namespace Microsoft.StandardUI.Controls   
{
    [UIModelObject]
    public interface IRow : IRowDefinition
    {
        IUICollection<IUIElement> Children { get; }
    }
}
