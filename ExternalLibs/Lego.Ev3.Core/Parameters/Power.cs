// <copyright file="Power.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Parameters
{
    using System;

    /// <summary>
    /// Represent a power parameter for a command to be send to the EV3 brick.
    /// </summary>
    public struct Power : IParameter<Power>
    {
        /// <summary>
        /// The minimum value
        /// </summary>
        public static readonly Power MinValue = -100;

        /// <summary>
        /// The maximum value
        /// </summary>
        public static readonly Power MaxValue = 100;

        /// <summary>
        /// The value
        /// </summary>
        internal int Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Power"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Power(int value)
        {
            Value = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="Power"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Power(int value)
        {
            return new Power(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Power"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator int(Power p)
        {
            return p.Value;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="Power"/> to <see cref="byte"/>.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator byte(Power p)
        {
            return Convert.ToByte(p.Value);
        }

        /// <inheritdoc />
        public int CompareTo(Power other)
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

            if (!(obj is Power))
            {
                throw new ArgumentException("Must be an Power", nameof(obj));
            }

            return CompareTo((Power)obj);
        }

        /// <inheritdoc />
        public bool Equals(Power other)
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

            if (!(obj is Power))
            {
                return false;
            }

            return Equals((Power)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("Power: {0}", Value);
        }
    }
}
