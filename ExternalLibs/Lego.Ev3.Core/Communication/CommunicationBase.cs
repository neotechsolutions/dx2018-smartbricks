// <copyright file="CommunicationBase.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Communication
{
    using System;
    using System.Threading.Tasks;
    using Lego.Ev3.Core.Enums;
    using Lego.Ev3.Core.Events;
    using Lego.Ev3.Core.Exceptions;
    using Lego.Ev3.Core.Interfaces;

    /// <summary>
    /// Base class for the <see cref="ICommunication"/> interface.
    /// </summary>
    public abstract class CommunicationBase : ICommunication
    {
        /// <inheritdoc />
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        /// <summary>
        /// Gets the type of the connection.
        /// </summary>
        protected abstract ConnectionType ConnectionType { get; }

        /// <summary>
        /// Connect to the EV3 brick
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task ConnectAsync();

        /// <summary>
        /// Disconnect from the EV3 brick.
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// Write data into the EV3 brick.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>A task.</returns>
        public abstract Task WriteAsync(byte[] data);

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            Disconnect();
        }

        /// <summary>
        /// Raises the data received event.
        /// </summary>
        /// <param name="data">The data.</param>
        protected virtual void RaiseDataReceived(byte[] data)
        {
            DataReceived?.Invoke(this, new DataReceivedEventArgs { Data = data });
        }

        /// <summary>
        /// Throws a <see cref="CommunicationErrorException"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        protected virtual void ThrowException(string message)
        {
            throw new CommunicationErrorException(message) { ConnectionType = ConnectionType };
        }
    }
}
