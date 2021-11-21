using Microsoft.StandardUI.SourceGenerator.UIFrameworks;
using System.IO;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class StandardModelUIFramework : UIFramework
    {
        public StandardModelUIFramework(Context context) : base(context)
        {
        }

        public override string ProjectBaseDirectory => Path.Combine("StandardUI", "StandardModel");
        public override string RootNamespace => "Microsoft.StandardUI.StandardModel";
        public override string FrameworkTypeForUIElementAttachedTarget => "ObjectWithCascadingNotifications";
        public override string? DefaultBaseClassName => "ObjectWithCascadingNotifications";
        public override string DefaultUIElementBaseClassName => "ObjectWithCascadingNotifications";
    }
}
