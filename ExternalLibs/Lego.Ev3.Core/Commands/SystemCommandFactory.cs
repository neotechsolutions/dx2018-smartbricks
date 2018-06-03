// <copyright file="SystemCommandFactory.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Commands
{
    using System;
    using System.Threading.Tasks;
    using Lego.Ev3.Core.Exceptions;
    using Lego.Ev3.Core.Extensions;
    using Lego.Ev3.Core.Interfaces;

    /// <summary>
    /// Factory for system commands to be send to the EV3 brick
    /// </summary>
    public sealed class SystemCommandFactory : CommandFactoryBase
    {
        private readonly IFileProvider _fileProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemCommandFactory"/> class.
        /// </summary>
        /// <param name="brick">The brick.</param>
        /// <param name="fileProvider">The file provider.</param>
        internal SystemCommandFactory(Brick brick, IFileProvider fileProvider)
            : base(brick)
        {
            _fileProvider = fileProvider;
        }

        /// <summary>
        /// Write a file to the EV3 brick
        /// </summary>
        /// <param name="data">Data to write.</param>
        /// <param name="devicePath">Destination path on the brick.</param>
        /// <returns>A task.</returns>
        /// <remarks>devicePath is relative from "lms2012/sys" on the EV3 brick.  Destination folders are automatically created if provided in the path.  The path must start with "apps", "prjs", or "tools".</remarks>
        public Task WriteFileAsync(byte[] data, string devicePath)
        {
            return WriteFileAsyncInternal(data, devicePath);
        }

        /// <summary>
        /// Copy a local file to the EV3 brick
        /// </summary>
        /// <param name="localPath">Source path on the computer.</param>
        /// <param name="devicePath">Destination path on the brick.</param>
        /// <returns>A task.</returns>
        /// <remarks>devicePath is relative from "lms2012/sys" on the EV3 brick.  Destination folders are automatically created if provided in the path.  The path must start with "apps", "prjs", or "tools".</remarks>
        public Task CopyFileAsync(string localPath, string devicePath)
        {
            return CopyFileAsyncInternal(localPath, devicePath);
        }

        /// <summary>
        /// Create a directory on the EV3 brick
        /// </summary>
        /// <param name="devicePath">Destination path on the brick.</param>
        /// <returns>A task.</returns>
        /// <remarks>devicePath is relative from "lms2012/sys" on the EV3 brick.  Destination folders are automatically created if provided in the path.  The path must start with "apps", "prjs", or "tools".</remarks>
        public Task CreateDirectoryAsync(string devicePath)
        {
            return CreateDirectoryAsyncInternal(devicePath);
        }

        /// <summary>
        /// Delete file from the EV3 brick
        /// </summary>
        /// <param name="devicePath">Destination path on the brick.</param>
        /// <returns>A task.</returns>
        /// <remarks>devicePath is relative from "lms2012/sys" on the EV3 brick.  The path must start with "apps", "prjs", or "tools".</remarks>
        public Task DeleteFileAsync(string devicePath)
        {
            return DeleteFileAsyncInternal(devicePath);
        }

        internal async Task DeleteFileAsyncInternal(string devicePath)
        {
            using (var ctx = CreateContext(CommandType.SystemReply))
            {
                ctx.Command.DeleteFile(devicePath);
                await ctx.ExecuteCommandAsync();

                var replyStatus = ctx.Command.Response.SystemReplyStatus;
                if (replyStatus != SystemReplyStatus.Success)
                {
                    throw new InvalidSystemReplyException("Error deleting file: {0}", replyStatus);
                }
            }
        }

        internal async Task CreateDirectoryAsyncInternal(string devicePath)
        {
            using (var ctx = CreateContext(CommandType.SystemReply))
            {
                ctx.Command.CreateDirectory(devicePath);
                await ctx.ExecuteCommandAsync();

                var replyStatus = ctx.Command.Response.SystemReplyStatus;
                if (replyStatus != SystemReplyStatus.Success)
                {
                    throw new InvalidSystemReplyException("Error creating directory: {0}", replyStatus);
                }
            }
        }

        internal async Task CopyFileAsyncInternal(string localPath, string devicePath)
        {
            byte[] data = await _fileProvider.GetFileContentAsync(localPath);
            await WriteFileAsyncInternal(data, devicePath);
        }

        internal async Task WriteFileAsyncInternal(byte[] data, string devicePath)
        {
            const int chunkSize = 960;

            byte fileHandle;

            using (var beginCtx = CreateContext(CommandType.SystemReply))
            {
                beginCtx.Command.AddOpcode(SystemOpcode.BeginDownload);
                beginCtx.Command.AddRawParameter((uint)data.Length);
                beginCtx.Command.AddRawParameter(devicePath);

                await beginCtx.ExecuteCommandAsync();

                var beginReplyStatus = beginCtx.Command.Response.SystemReplyStatus;
                if (beginReplyStatus != SystemReplyStatus.Success)
                {
                    throw new InvalidSystemReplyException("Could not begin file save: {0}", beginReplyStatus);
                }

                fileHandle = beginCtx.Command.Response.Data[0];
            }

            int sizeSent = 0;

            while (sizeSent < data.Length)
            {
                int sizeToSend = Math.Min(chunkSize, data.Length - sizeSent);

                using (var continueCtx = CreateContext(CommandType.SystemReply))
                {
                    continueCtx.Command.AddOpcode(SystemOpcode.ContinueDownload);
                    continueCtx.Command.AddRawParameter(fileHandle);
                    continueCtx.Command.AddRawParameter(data, sizeSent, sizeToSend);
                    sizeSent += sizeToSend;

                    await continueCtx.ExecuteCommandAsync();

                    var continueReplyStatus = continueCtx.Command.Response.SystemReplyStatus;
                    if (continueReplyStatus != SystemReplyStatus.Success
                        && (continueReplyStatus != SystemReplyStatus.EndOfFile && sizeSent == data.Length))
                    {
                        throw new InvalidSystemReplyException("Error saving file: {0}", continueReplyStatus);
                    }
                }
            }

            // Command commandClose = new Command(CommandType.SystemReply);
            // commandClose.AddOpcode(SystemOpcode.CloseFileHandle);
            // commandClose.AddRawParameter(handle);
            // await _brick.SendCommandAsyncInternal(commandClose);
            // if(commandClose.Response.SystemReplyStatus != SystemReplyStatus.Success)
            //  throw new Exception("Could not close handle: " + commandClose.Response.SystemReplyStatus);
        }
    }
}
