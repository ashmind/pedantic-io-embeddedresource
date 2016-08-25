using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Pedantic.IO.Internal {
    internal static class Argument {
        public static void RequireNotNull([InvokerParameterName] string name, [NotNull] object value) {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        public static void RequireNotNullOrEmpty([InvokerParameterName] string name, [NotNull] string value) {
            RequireNotNull(name, value);
            if (value.Length == 0)
                throw new ArgumentException("Value cannot be empty.", name);
        }
    }
}
