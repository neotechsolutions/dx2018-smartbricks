// <copyright file="BrickChangedEventArgs.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Events
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Arguments for the brick changed event
    /// </summary>
    public sealed class BrickChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrickChangedEventArgs"/> class.
        /// </summary>
        /// <param name="ports">The ports.</param>
        /// <param name="buttons">The buttons.</param>
        internal BrickChangedEventArgs(IDictionary<InputPort, Port> ports, BrickButtons buttons)
        {
            Ports = ports;
            Buttons = buttons;
        }

        /// <summary>
        /// Gets a map of all ports on the EV3 brick
        /// </summary>
        public IDictionary<InputPort, Port> Ports { get; }

        /// <summary>
        /// Gets the buttons on the face of the EV3 brick
        /// </summary>
        public BrickButtons Buttons { get; }
    }
}