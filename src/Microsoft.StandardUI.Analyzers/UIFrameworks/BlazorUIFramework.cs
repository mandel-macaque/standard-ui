namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public class BlazorUIFramework : NonXamlUIFramework
    {
        public BlazorUIFramework(Context context) : base(context)
        {
        }

        public override string Name => "Blazor";
        public override string FrameworkTypeForUIElementAttachedTarget => "Microsoft.AspNetCore.Components.ComponentBase";
        public override string NativeUIElementType => "";   // TODO: Supply right value here
        protected override string FontFamilyDefaultValue => "\"\""; // TODO: Supply right value here
    }
}
