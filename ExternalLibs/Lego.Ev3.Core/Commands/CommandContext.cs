// <copyright file="CommandContext.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Commands
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Execution context for a command.
    /// </summary>
    public sealed class CommandContext : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContext"/> class.
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="brick">The brick.</param>
        internal CommandContext(Command c, Brick brick)
        {
            Command = c;
            Brick = brick;
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        public Command Command { get; }

        /// <summary>
        /// Gets the brick.
        /// </summary>
        private Brick Brick { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            Command?.Dispose();
        }

        /// <summary>
        /// Execute the command in the current context.
        /// </summary>
        /// <returns>A task.</returns>
        public Task ExecuteCommandAsync()
        {
            return Brick.SendCommandAsyncInternal(Command);
        }
    }
}
