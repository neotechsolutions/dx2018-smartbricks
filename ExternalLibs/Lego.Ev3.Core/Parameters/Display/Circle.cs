﻿// <copyright file="Circle.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Parameters.Display
{
    /// <summary>
    /// Represent a circle to be displayed on the LCD screen of the EV3 brick.
    /// </summary>
    public struct Circle : IGeometry
    {
        /// <summary>
        /// Gets or sets the position on X-axis.
        /// </summary>
        public ushort X { get; set; }

        /// <summary>
        /// Gets or sets the position on Y-axis.
        /// </summary>
        public ushort Y { get; set; }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        public ushort Radius { get; set; }
    }
}
