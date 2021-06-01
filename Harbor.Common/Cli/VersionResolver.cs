using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Common.Cli
{
    /// <summary>
    /// Represents a version resolver.
    /// </summary>
    public interface IVersionResolver
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <returns>The version.</returns>
        string GetVersion();

        /// <summary>
        /// Gets the product version.
        /// </summary>
        /// <returns>The product version.</returns>
        string GetProductVersion();
    }

    /// <summary>
    /// The Cake version resolver.
    /// </summary>
    public sealed class VersionResolver : IVersionResolver
    {
        /// <inheritdoc/>
        public string GetVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            var version = FileVersionInfo.GetVersionInfo(assembly!.Location).Comments;

            if (string.IsNullOrWhiteSpace(version))
            {
                version = "Unknown";
            }

            return version;
        }

        public static string GetVersion2()
        {
            var res = new VersionResolver();
            return res.GetVersion();
        }

        /// <inheritdoc/>
        public string GetProductVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            var version = FileVersionInfo.GetVersionInfo(assembly!.Location).ProductVersion;

            if (string.IsNullOrWhiteSpace(version))
            {
                version = "Unknown";
            }

            return version;
        }
    }
}
