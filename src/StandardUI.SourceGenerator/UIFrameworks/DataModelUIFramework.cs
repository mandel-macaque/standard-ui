using System.IO;

namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public class DataModelUIFramework : UIFramework
    {
        public DataModelUIFramework(Context context) : base(context)
        {
        }

        public override string ProjectBaseDirectory => Path.Combine("StandardUI", "DataModel");
        public override string RootNamespace => "Microsoft.StandardUI.DataModel";
        public override string FrameworkTypeForUIElementAttachedTarget => "ObjectWithCascadingNotifications";
        public override string? DefaultBaseClassName => "ObjectWithCascadingNotifications";
        public override string DefaultUIElementBaseClassName => "ObjectWithCascadingNotifications";
        protected override string FontFamilyDefaultValue => throw new System.NotImplementedException();
    }
}
