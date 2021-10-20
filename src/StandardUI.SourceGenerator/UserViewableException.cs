using System;

namespace Microsoft.StandardUI.SourceGenerator
{
    public class UserViewableException : Exception
    {
        public UserViewableException(string message) : base(message)
        {
        }
    }
}