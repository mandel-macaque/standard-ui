namespace Microsoft.StandardUI.Controls   
{
    public interface IRow : IRowDefinition
    {
        IUICollection<IUIElement> Children { get; }
    }
}
