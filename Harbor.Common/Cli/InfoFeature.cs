using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;

namespace Harbor.Common.Cli
{
    public interface IHarborInfoFeature
    {
        void Run(IConsole console);
    }

    public sealed class InfoFeature : IHarborInfoFeature
    {
        private readonly IVersionResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoFeature"/> class.
        /// </summary>
        /// <param name="resolver">The version resolver.</param>
        public InfoFeature(IVersionResolver resolver)
        {
            _resolver = resolver;
        }

        public void Run(IConsole console)
        {
            var version = _resolver.GetVersion();
            var product = _resolver.GetProductVersion();

            console.ForegroundColor = System.ConsoleColor.Yellow;
            console.WriteLine(@"Harbor");
            console.ResetColor();

            console.WriteLine();
            console.WriteLine(@"Version: {0}", version);
            console.WriteLine(@"Details: {0}", string.Join("\n         ", product.Split('/')));
            console.WriteLine();
        }
    }
}
