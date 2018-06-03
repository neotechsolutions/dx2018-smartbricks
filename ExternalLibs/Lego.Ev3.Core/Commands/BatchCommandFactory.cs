// <copyright file="BatchCommandFactory.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Commands
{
    using System;
    using System.Threading.Tasks;
    using Lego.Ev3.Core.Extensions;

    /// <summary>
    /// Factory for a chain of commands to be written to the EV3 brick in batch.
    /// </summary>
    public sealed class BatchCommandFactory : CommandFactoryBase
    {
        private CommandContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCommandFactory"/> class.
        /// </summary>
        /// <param name="brick">The brick.</param>
        internal BatchCommandFactory(Brick brick)
            : base(brick)
        {
        }

        /// <summary>
        /// Gets the inner command to be chained.
        /// </summary>
        public Command InnerCommand
        {
            get
            {
                if (_context == null)
                {
                    Initialize(CommandType.DirectNoReply);
                }

                return _context.Command;
            }
        }

        /// <summary>
        /// Start a new command of a specific type
        /// </summary>
        /// <param name="commandType">The type of the command to start</param>
        /// <returns>The command context.</returns>
        public CommandContext Initialize(CommandType commandType)
        {
            return Initialize(commandType, 0, 0);
        }

        /// <summary>
        /// Start a new command of a specific type with a global and/or local buffer on the EV3 brick
        /// </summary>
        /// <param name="commandType">The type of the command to start</param>
        /// <param name="globalSize">The size of the global buffer in bytes (maximum of 1024 bytes)</param>
        /// <param name="localSize">The size of the local buffer in bytes (maximum of 64 bytes)</param>
        /// <returns>The command context.</returns>
        public CommandContext Initialize(CommandType commandType, ushort globalSize, int localSize)
        {
            if (_context != null)
            {
                _context.Dispose();
            }

            _context = CreateContext(commandType, globalSize, localSize);
            return _context;
        }

        /// <summary>
        /// End and send a Command to the EV3 brick.
        /// </summary>
        /// <returns>A byte array containing the response from the brick, if any.</returns>
        public async Task<byte[]> SendCommandAsync()
        {
            await _context.ExecuteCommandAsync();
            byte[] response = _context.Command.Response.Data;

            _context.Dispose();
            _context = null;

            return response;
        }
    }
}
