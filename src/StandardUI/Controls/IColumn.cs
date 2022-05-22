namespace Microsoft.StandardUI.Controls   
{
    [UIModelObject]
    public interface IColumn : IColumnDefinition
    {
        IUICollection<IUIElement> Children { get; }
    }
}
