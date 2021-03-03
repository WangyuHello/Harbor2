using Cake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Cli
{
    /// <summary>
    /// Represents a feature that writes the Cake version to the console.
    /// </summary>
    public interface IHarborVersionFeature
    {
        /// <summary>
        /// Writes the Cake version to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        void Run(IConsole console);
    }

    /// <summary>
    /// Writes the Cake version to the console.
    /// </summary>
    public sealed class VersionFeature : IHarborVersionFeature
    {
        private readonly IVersionResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionFeature"/> class.
        /// </summary>
        /// <param name="resolver">The version resolver.</param>
        public VersionFeature(IVersionResolver resolver)
        {
            _resolver = resolver;
        }

        /// <inheritdoc/>
        public void Run(IConsole console)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.WriteLine(_resolver.GetVersion());
        }
    }
}
