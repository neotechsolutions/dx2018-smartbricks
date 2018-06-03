// <copyright file="TurnRatio.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Parameters
{
    using System;

    /// <summary>
    /// Represent a turn ratio parameter for a command to be send to the EV3 brick.
    /// </summary>
    public struct TurnRatio : IParameter<TurnRatio>
    {
        /// <summary>
        /// The minimum value
        /// </summary>
        public static readonly TurnRatio MinValue = -200;

        /// <summary>
        /// The maximum value
        /// </summary>
        public static readonly TurnRatio MaxValue = 200;

        /// <summary>
        /// The value
        /// </summary>
        internal short Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="TurnRatio"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public TurnRatio(short value)
        {
            Value = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="short"/> to <see cref="TurnRatio"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator TurnRatio(short value)
        {
            return new TurnRatio(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="TurnRatio"/> to <see cref="short"/>.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator short(TurnRatio p)
        {
            return p.Value;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="TurnRatio"/> to <see cref="byte"/>.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator byte(TurnRatio p)
        {
            return Convert.ToByte(p.Value);
        }

        /// <inheritdoc />
        public int CompareTo(TurnRatio other)
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

            if (!(obj is TurnRatio))
            {
                throw new ArgumentException("Must be an TurnRatio", nameof(obj));
            }

            return CompareTo((TurnRatio)obj);
        }

        /// <inheritdoc />
        public bool Equals(TurnRatio other)
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

            if (!(obj is TurnRatio))
            {
                return false;
            }

            return Equals((TurnRatio)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("Turn ratio: {0}", Value);
        }
    }
}
