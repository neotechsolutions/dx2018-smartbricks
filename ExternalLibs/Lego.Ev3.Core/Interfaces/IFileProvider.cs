// <copyright file="IFileProvider.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for accessing File System.
    /// </summary>
    public interface IFileProvider
    {
        /// <summary>
        /// Gets the content of the file at the specified local path.
        /// </summary>
        /// <param name="localPath">The local path.</param>
        /// <returns>The content.</returns>
        Task<byte[]> GetFileContentAsync(string localPath);
    }
}
