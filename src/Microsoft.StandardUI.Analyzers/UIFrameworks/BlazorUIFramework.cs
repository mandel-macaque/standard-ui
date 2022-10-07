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

        public override void GeneratePropertyAttribute(Property property, Source source)
        {
            if (! property.IsReadOnly)
            {
                source.Usings.AddNamespace("Microsoft.AspNetCore.Components");
                source.AddLine("[Parameter]");
            }
        }

        /* TODOS:
         * Ensure that the DrawableTree is only generated on Shapes/Drawable classes only
         * Make the drawable Tree dimensions match that of the image canvas
         * Pass the SKPicture and necessary info to the Blazor classes
         */
    }
}