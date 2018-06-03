﻿// <copyright file="Line.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Parameters.Display
{
    /// <summary>
    /// Represent a line to be displayed on the LCD screen of the EV3 brick.
    /// </summary>
    public struct Line
    {
        /// <summary>
        /// Gets or sets the start position on X-axis.
        /// </summary>
        public ushort X0 { get; set; }

        /// <summary>
        /// Gets or sets the start position on Y-axis.
        /// </summary>
        public ushort Y0 { get; set; }

        /// <summary>
        /// Gets or sets the end position on X-axis.
        /// </summary>
        public ushort X1 { get; set; }

        /// <summary>
        /// Gets or sets the end position on Y-axis.
        /// </summary>
        public ushort Y1 { get; set; }
    }
}
