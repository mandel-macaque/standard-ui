namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public abstract class UIFramework
    {
        public abstract string ProjectBaseDirectory { get; }
        public abstract string RootNamespace { get; }
        public abstract string FrameworkTypeForUIElementAttachedTarget { get; }
        public abstract string? DefaultBaseClassName { get; }
        public abstract string DefaultUIElementBaseClassName { get; }
        public virtual void AddUsings(Usings usings, bool hasPropertyDescriptors, bool hasTypeConverterAttribute) { }
        public virtual void AddTypeAliasUsingIfNeeded(Usings usings, string destinationTypeName) { }
        public virtual void GenerateStandardPanelLayoutMethods(Source methodsSource, string layoutManagerTypeName) { }

        public virtual void GeneratePropertyDescriptor(Property property, Source destinationStaticMembers) { }
        public virtual void GeneratePropertyField(Property property, Source nonstaticFields) { }
        public virtual void GeneratePropertyConstructorLines(Property property, Source constuctorBody) { }
        public virtual void GeneratePropertyMethods(Property property, Source source) { }
    }
}
