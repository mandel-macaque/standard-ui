// This file is generated from IPropertyPath.cs. Update the source file to change its contents.

namespace Microsoft.StandardUI.WinForms
{
    public class PropertyPath : StandardUIObject, IPropertyPath
    {
        public static readonly UIProperty PathProperty = new UIProperty(nameof(Path), "", readOnly:true);
        
        public string Path => (string) GetValue(PathProperty);
    }
}
