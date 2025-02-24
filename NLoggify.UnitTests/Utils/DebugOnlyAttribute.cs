using System.Diagnostics;

namespace NLoggify.UnitTests.Utils
{
    [Conditional("DEBUG")]
    public class DebugOnlyAttribute : Attribute
    {
        // Implementation of the attribute
    }
}
