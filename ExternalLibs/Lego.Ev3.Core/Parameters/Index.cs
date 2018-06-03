// <copyright file="Index.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Parameters
{
    using System;

    /// <summary>
    /// Represent an index parameter for a command to be send to the EV3 brick.
    /// </summary>
    public struct Index : IParameter<Index>
    {
        /// <summary>
        /// The minimum value
        /// </summary>
        public static readonly Index MinValue = uint.MinValue;

        /// <summary>
        /// The maximum value
        /// </summary>
        public static readonly Index MaxValue = 1024;

        /// <summary>
        /// The value
        /// </summary>
        internal uint Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Index"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Index(uint value)
        {
            Value = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="uint"/> to <see cref="Index"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Index(uint value)
        {
            return new Index(value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="int"/> to <see cref="Index"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator Index(int value)
        {
            return new Index(Convert.ToUInt32(value));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Index"/> to <see cref="uint"/>.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator uint(Index i)
        {
            return i.Value;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="Index"/> to <see cref="byte"/>.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator byte(Index i)
        {
            return Convert.ToByte(i.Value);
        }

        /// <inheritdoc />
        public int CompareTo(Index other)
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

            if (!(obj is Index))
            {
                throw new ArgumentException("Must be an Index", nameof(obj));
            }

            return CompareTo((Index)obj);
        }

        /// <inheritdoc />
        public bool Equals(Index other)
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

            if (!(obj is Index))
            {
                return false;
            }

            return Equals((Index)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("Index: {0}", Value);
        }
    }
}
