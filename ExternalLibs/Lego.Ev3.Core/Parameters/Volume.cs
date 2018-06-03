// <copyright file="Volume.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Parameters
{
    using System;

    /// <summary>
    /// Represent a volume parameter for a command to be send to the EV3 brick.
    /// </summary>
    public struct Volume : IParameter<Volume>
    {
        /// <summary>
        /// The minimum value
        /// </summary>
        public static readonly Volume MinValue = 0;

        /// <summary>
        /// The maximum value
        /// </summary>
        public static readonly Volume MaxValue = 100;

        /// <summary>
        /// The value
        /// </summary>
        internal int Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Volume"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Volume(int value)
        {
            Value = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="Volume"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Volume(int value)
        {
            return new Volume(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Volume"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator int(Volume p)
        {
            return p.Value;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="Volume"/> to <see cref="byte"/>.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator byte(Volume p)
        {
            return Convert.ToByte(p.Value);
        }

        /// <inheritdoc />
        public int CompareTo(Volume other)
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

            if (!(obj is Volume))
            {
                throw new ArgumentException("Must be an Volume", nameof(obj));
            }

            return CompareTo((Volume)obj);
        }

        /// <inheritdoc />
        public bool Equals(Volume other)
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

            if (!(obj is Volume))
            {
                return false;
            }

            return Equals((Volume)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("Volume: {0}", Value);
        }
    }
}
