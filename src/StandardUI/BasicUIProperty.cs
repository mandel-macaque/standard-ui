namespace Microsoft.StandardUI
{
    public struct BasicUIProperty : IUIProperty
    {
        private string _name;
        private object? _defaultValue;
        private bool _readOnly;

        public BasicUIProperty(string name, object? defaultValue, bool readOnly)
        {
            this._name = name;
            this._defaultValue = defaultValue;
            this._readOnly = readOnly;
        }
    }
}
