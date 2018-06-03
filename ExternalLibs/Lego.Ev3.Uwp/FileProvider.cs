// <copyright file="FileProvider.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Uwp
{
    using System;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading.Tasks;
    using Lego.Ev3.Core.Interfaces;
    using Windows.Storage;
    using Windows.Storage.Streams;

    /// <summary>
    /// Implementation of the <see cref="IFileProvider"/> interface for UWP.
    /// </summary>
    public class FileProvider : IFileProvider
    {
        /// <inheritdoc />
        public async Task<byte[]> GetFileContentAsync(string localPath)
        {
            StorageFile sf = await StorageFile.GetFileFromPathAsync(localPath);
            IBuffer buffer = await FileIO.ReadBufferAsync(sf);
            return buffer.ToArray();
        }
    }
}
