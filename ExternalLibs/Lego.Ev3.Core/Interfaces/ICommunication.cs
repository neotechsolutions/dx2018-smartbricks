// <copyright file="ICommunication.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Lego.Ev3.Core.Events;

    /// <summary>
    /// Interface for communicating with the EV3 brick
    /// </summary>
    public interface ICommunication : IDisposable
    {
        /// <summary>
        /// Fired when data have been received and are ready to parse and process.
        /// </summary>
        event EventHandler<DataReceivedEventArgs> DataReceived;

        /// <summary>
        /// Connect to the EV3 brick.
        /// </summary>
        /// <returns>A task.</returns>
        Task ConnectAsync();

        /// <summary>
        /// Disconnect from the EV3 brick.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Write data to the EV3 brick.
        /// </summary>
        /// <param name="data">The data to write.</param>
        /// <returns>A task.</returns>
        Task WriteAsync(byte[] data);
    }
}
