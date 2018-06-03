// <copyright file="BrickButtons.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core
{
    /// <summary>
    /// Buttons on the face of the LEGO EV3 brick
    /// </summary>
    public sealed class BrickButtons
    {
        /// <summary>
        /// Gets or sets a value indicating whether the up button is pressed
        /// </summary>
        public bool Up { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the down button is pressed
        /// </summary>
        public bool Down { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the left button is pressed
        /// </summary>
        public bool Left { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the right button is pressed
        /// </summary>
        public bool Right { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the back button is pressed
        /// </summary>
        public bool Back { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the enter button is pressed
        /// </summary>
        public bool Enter { get; set; }
    }
}