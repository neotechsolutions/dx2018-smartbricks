// <copyright file="CommandFactoryBase.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Commands
{
    using System.Threading.Tasks;

    /// <summary>
    /// Base factory of commands to be send using the EV3 brick
    /// </summary>
    public abstract class CommandFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandFactoryBase"/> class.
        /// </summary>
        /// <param name="brick">The brick.</param>
        protected CommandFactoryBase(Brick brick)
        {
            Brick = brick;
        }

        /// <summary>
        /// Gets the brick.
        /// </summary>
        protected Brick Brick { get; }

        /// <summary>
        /// Create the context for the execution of the command.
        /// </summary>
        /// <param name="commandType">The type of the command to start</param>
        /// <param name="globalSize">The size of the global buffer in bytes (maximum of 1024 bytes)</param>
        /// <param name="localSize">The size of the local buffer in bytes (maximum of 64 bytes)</param>
        /// <returns>The created context.</returns>
        internal CommandContext CreateContext(CommandType commandType, ushort globalSize = 0, int localSize = 0)
        {
            var command = new Command(commandType, globalSize, localSize);
            return new CommandContext(command, Brick);
        }
    }
}
