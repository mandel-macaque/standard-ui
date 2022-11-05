using System;

namespace Microsoft.ComponentModelEx.ReactiveData {
    internal static class RunningDerivationsStack {
        [ThreadStatic] internal static RunningDerivation Top;
    }
}
