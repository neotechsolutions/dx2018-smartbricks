// <copyright file="Lcd.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Contains constants for the LCD screen
    /// </summary>
    public static class Lcd
    {
        /// <summary>
        /// Width of LCD screen
        /// </summary>
        public const ushort Width = 178;

        /// <summary>
        /// Height of LCD screen
        /// </summary>
        public const ushort Height = 128;

        /// <summary>
        /// Height of status bar
        /// </summary>
        public const ushort TopLineHeight = 10;
    }
}
