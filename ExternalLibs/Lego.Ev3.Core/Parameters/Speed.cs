// <copyright file="Speed.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Parameters
{
    using System;

    /// <summary>
    /// Represent a speed parameter for a command to be send to the EV3 brick.
    /// </summary>
    public struct Speed : IParameter<Speed>
    {
        /// <summary>
        /// The minimum value
        /// </summary>
        public static readonly Speed MinValue = -100;

        /// <summary>
        /// The maximum value
        /// </summary>
        public static readonly Speed MaxValue = 100;

        /// <summary>
        /// The value
        /// </summary>
        internal int Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Speed"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Speed(int value)
        {
            Value = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="Speed"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Speed(int value)
        {
            return new Speed(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Speed"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator int(Speed p)
        {
            return p.Value;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="Speed"/> to <see cref="byte"/>.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator byte(Speed p)
        {
            return Convert.ToByte(p.Value);
        }

        /// <inheritdoc />
        public int CompareTo(Speed other)
        {
            return Value.CompareTo(other.Value);
        }

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (!(obj is Speed))
            {
                throw new ArgumentException("Must be an Speed", nameof(obj));
            }

            return CompareTo((Speed)obj);
        }

        /// <inheritdoc />
        public bool Equals(Speed other)
        {
            return Value.Equals(other.Value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Speed))
            {
                return false;
            }

            return Equals((Speed)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("Speed: {0}", Value);
        }
    }
}
