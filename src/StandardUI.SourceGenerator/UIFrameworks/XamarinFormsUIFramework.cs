namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public class XamarinFormsUIFramework : XamlUIFramework
    {
        public XamarinFormsUIFramework(Context context) : base(context)
        {
        }

        public override string ProjectBaseDirectory => "StandardUI.XamarinForms";
        public override string RootNamespace => "Microsoft.StandardUI.XamarinForms";
        public override string DependencyPropertyClassName => "BindableProperty";
        public override string FrameworkTypeForUIElementAttachedTarget => "VisualElement";
        public override string? DefaultBaseClassName => "StandardUIBindableObject";
        public override string DefaultUIElementBaseClassName => "StandardUIView";
        public override string WrapperSuffix => "XamarinForms";
        protected override string FontFamilyDefaultValue => throw new System.NotImplementedException();

        public override void AddTypeAliasUsingIfNeeded(Usings usings, string destinationTypeName)
        {
            // These types are also defined in Xamarin.Forms, so add aliases to prefer the Standard UI type
            if (destinationTypeName == "Brush" || destinationTypeName == "Brush?")
                usings.AddTypeAlias("Brush = Microsoft.StandardUI.XamarinForms.Media.Brush");
            else if (destinationTypeName == "SweepDirection")
                usings.AddTypeAlias("SweepDirection = Microsoft.StandardUI.Media.SweepDirection");
        }
    }
}
