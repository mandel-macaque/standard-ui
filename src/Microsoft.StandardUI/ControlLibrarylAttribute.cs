using System;

namespace Microsoft.StandardUI
{
    /// <summary>
    /// Designate the assembly as containing .NET Standard Controls and specify
    /// associated metadata for the library.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ControlLibraryAttribute : Attribute
    {
        public string Name { get; }

        /// <summary>
        /// Designnate the assembly as containing .NET Standard Controls and 
        /// specify library metadata like the library name.
        /// 
        /// The name should be fully qualified, with the namespace + friendly
        /// library name, for instance "MyCompany.FancyGauges".
        /// The library name is used to form class names for generated code applying
        /// to the whole library (e.g. "FancyGaugesStatics") and the namespace
        /// gives the namespace for such generated classes; it's normally the root
        /// namespace for the library.
        /// </summary>
        /// <param name="name">fully qualified name of the library</param>
        public ControlLibraryAttribute(string name)
        {
            Name = name;
        }
    }
}
