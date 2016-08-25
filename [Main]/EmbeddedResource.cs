using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;
using Pedantic.IO.Internal;

namespace Pedantic.IO {
    [PublicAPI]
    public static class EmbeddedResource {
        [NotNull, Pure]
        public static string ReadAllText([NotNull] Assembly assembly, [NotNull] string absoluteName) {
            Argument.RequireNotNull(nameof(assembly), assembly);
            Argument.RequireNotNullOrEmpty(nameof(absoluteName), absoluteName);

            using (var stream = OpenRead(assembly, absoluteName))
            using (var reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }

        [NotNull, Pure]
        public static string ReadAllText([NotNull] Type typeInParentNamespace, [NotNull] string relativeName) {
            Argument.RequireNotNull(nameof(typeInParentNamespace), typeInParentNamespace);
            Argument.RequireNotNullOrEmpty(nameof(relativeName), relativeName);

            return ReadAllText(GetAssembly(typeInParentNamespace), typeInParentNamespace.Namespace + "." + relativeName);
        }

        [NotNull, Pure]
        public static byte[] ReadAllBytes([NotNull] Assembly assembly, [NotNull] string absoluteName) {
            Argument.RequireNotNull(nameof(assembly), assembly);
            Argument.RequireNotNullOrEmpty(nameof(absoluteName), absoluteName);

            using (var stream = OpenRead(assembly, absoluteName)) {
                if (stream.Length > int.MaxValue)
                    throw new NotSupportedException($"Resource '{absoluteName}' exceeds maximum supported size of '{int.MaxValue:N0}'");

                using (var reader = new BinaryReader(stream)) {
                    return reader.ReadBytes((int)stream.Length);
                }
            }
        }

        [NotNull, Pure]
        public static byte[] ReadAllBytes([NotNull] Type typeInParentNamespace, [NotNull] string relativeName) {
            Argument.RequireNotNull(nameof(typeInParentNamespace), typeInParentNamespace);
            Argument.RequireNotNullOrEmpty(nameof(relativeName), relativeName);

            return ReadAllBytes(GetAssembly(typeInParentNamespace), typeInParentNamespace.Namespace + "." + relativeName);
        }

        [NotNull, Pure]
        public static Stream OpenRead([NotNull] Assembly assembly, [NotNull] string absoluteName) {
            Argument.RequireNotNull(nameof(assembly), assembly);
            Argument.RequireNotNullOrEmpty(nameof(absoluteName), absoluteName);

            var stream = assembly.GetManifestResourceStream(absoluteName);
            if (stream == null)
                throw new ResourceNotFoundException($"Resource '{absoluteName}' was not found in assembly.", assembly, absoluteName);

            return stream;
        }

        [NotNull, Pure]
        public static Stream OpenRead([NotNull] Type typeInParentNamespace, [NotNull] string relativeName) {
            Argument.RequireNotNull(nameof(typeInParentNamespace), typeInParentNamespace);
            Argument.RequireNotNullOrEmpty(nameof(relativeName), relativeName);

            return OpenRead(GetAssembly(typeInParentNamespace), typeInParentNamespace.Namespace + "." + relativeName);
        }

        private static Assembly GetAssembly(Type type) {
        #if NETCORE
            return type.GetTypeInfo().Assembly;
        #else
            return type.Assembly;
        #endif
        }
    }
}
