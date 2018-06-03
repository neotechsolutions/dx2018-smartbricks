// <copyright file="ConnectionType.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Enums
{
    /// <summary>
    /// The available types of connection between a client and the EV3 brick
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// Unknown type
        /// </summary>
        Unknown,

        /// <summary>
        /// Bluetooth connection
        /// </summary>
        Bluetooth,

        /// <summary>
        /// USB connection
        /// </summary>
        Usb,

        /// <summary>
        /// Network (Wifi) connection
        /// </summary>
        Network
    }
}
