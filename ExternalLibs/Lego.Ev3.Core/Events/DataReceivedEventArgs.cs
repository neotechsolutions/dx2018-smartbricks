// <copyright file="DataReceivedEventArgs.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Events
{
    using System;

    /// <summary>
    /// Event arguments for the DataReceived event.
    /// </summary>
    public sealed class DataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the byte array of the data received from the EV3 brick.
        /// </summary>
        public byte[] Data { get; set; }
    }
}