using System;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;

namespace Pedantic.IO {
    [PublicAPI]
    public class ResourceNotFoundException : FileNotFoundException {
        public ResourceNotFoundException([CanBeNull] string message, [CanBeNull] Assembly assembly, [CanBeNull] string resourceName) : base(message, resourceName) {
            Assembly = assembly;
        }

        public ResourceNotFoundException([CanBeNull] string message, [CanBeNull] Assembly assembly, [CanBeNull] string resourceName, [CanBeNull] Exception innerException) : base(message, resourceName, innerException) {
            Assembly = assembly;
        }

        [CanBeNull] public Assembly Assembly { get; }
        [CanBeNull] public string ResourceName => FileName;
    }
}