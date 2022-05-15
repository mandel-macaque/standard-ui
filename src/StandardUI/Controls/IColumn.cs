namespace Microsoft.StandardUI.Controls   
{
    public interface IColumn : IColumnDefinition
    {
        IUICollection<IUIElement> Children { get; }
    }
}
