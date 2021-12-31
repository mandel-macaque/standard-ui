namespace Microsoft.StandardUI.Mac
{
    public interface ILogicalParent
    {
        void AddLogicalChild(object child);

        void RemoveLogicalChild(object child);
    }
}
