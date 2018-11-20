using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Nomailme.ConfigurationPrinter
{
    /// <summary>
    /// <see cref="Nomailme.ConfigurationPrinter"/> configuration.
    /// </summary>
    public class ConfigurationPrinterOptions
    {
        /// <summary>
        /// Skip printing of microsoft configured options.
        /// </summary>
        /// <remarks>
        /// If set to true, it will print tons of useless info.
        /// </remarks>
        public bool IgnoreMicrosoftOptions { get; set; } = true;

        public List<string> MaskedProperties { get; set; } = new List<string>();

        /// <summary>
        /// Skip printing large <see cref="IOptions{TOptions}"/>.
        /// </summary>
        public int? MaxOptionsLength { get; set; }
    }
}
