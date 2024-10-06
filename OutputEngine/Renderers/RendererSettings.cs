// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace OutputEngine.Renderers
{
    public class RendererSettings
    {
        /// <summary>
        /// Create a new instance of <see cref="RendererSettings"/>
        /// </summary>
        /// <param name="shouldRedirect">Whether output should be redirected to something other than StdOut and StdError for testing, or possibly some other uses.</param>
        /// <param name="writer">The TextWriter to use.</param>
        /// <remarks>
        /// We need both ShouldRedirect and Writer because Spectre needs the Boolean and a writer
        /// and direct the other renderers need a Writer. Other renderers may be interested in whether
        /// output is redirected, so settings are expected to set both, although in the case of Spectre
        /// testing (Should redirect == true), the writer will be null.
        /// </remarks>
        public RendererSettings(bool shouldRedirect, CliWriter writer)
        {
            ShouldRedirect = shouldRedirect;
            Writer = writer;
        }

        public bool ShouldRedirect { get; set; }
        public CliWriter? Writer { get; }
    }
}