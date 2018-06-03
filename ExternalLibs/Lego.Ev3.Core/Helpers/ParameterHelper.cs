// <copyright file="ParameterHelper.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Helpers
{
    using System;
    using Lego.Ev3.Core.Parameters;

    /// <summary>
    /// Helper class for management of command parameters
    /// </summary>
    internal static class ParameterHelper
    {
        /// <summary>
        /// Checks whether the specified index is in range.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="usage">The usage.</param>
        internal static void VerifyIndexValid(Index index, string parameterName = null, string usage = null)
        {
            if (index.CompareTo(Index.MaxValue) > 0)
            {
                throw new ArgumentException($"{usage ?? "Index"} cannot be greater than {Index.MaxValue.Value}.", parameterName ?? nameof(index));
            }
        }

        /// <summary>
        /// Checks whether the specified power is in range.
        /// </summary>
        /// <param name="power">The power.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="usage">The usage.</param>
        internal static void VerifyPowerInRange(Power power, string parameterName = null, string usage = null)
        {
            if (power.CompareTo(Power.MinValue) < 0 || power.CompareTo(Power.MaxValue) > 0)
            {
                throw new ArgumentException($"{usage ?? "Power"} must be between {Power.MinValue.Value} and {Power.MaxValue.Value} inclusive.", parameterName ?? nameof(power));
            }
        }

        /// <summary>
        /// Checks whether the specified speed is in range.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="usage">The usage.</param>
        internal static void VerifySpeedInRange(Speed speed, string parameterName = null, string usage = null)
        {
            if (speed.CompareTo(Speed.MinValue) < 0 || speed.CompareTo(Speed.MaxValue) > 0)
            {
                throw new ArgumentException($"{usage ?? "Speed"} must be between {Speed.MinValue.Value} and {Speed.MaxValue.Value} inclusive.", parameterName ?? nameof(speed));
            }
        }

        /// <summary>
        /// Checks whether the specified turn ratio is in range.
        /// </summary>
        /// <param name="turnRatio">The turn ratio.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="usage">The usage.</param>
        internal static void VerifyTurnRatioInRange(TurnRatio turnRatio, string parameterName = null, string usage = null)
        {
            if (turnRatio.CompareTo(TurnRatio.MinValue) < 0 || turnRatio.CompareTo(TurnRatio.MaxValue) > 0)
            {
                throw new ArgumentException($"{usage ?? "Turn ratio"} must be between {TurnRatio.MinValue.Value} and {TurnRatio.MaxValue.Value} inclusive.", parameterName ?? nameof(turnRatio));
            }
        }

        /// <summary>
        /// Checks whether the specified volume is in range.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="usage">The usage.</param>
        internal static void VerifyVolumeInRange(Volume volume, string parameterName = null, string usage = null)
        {
            if (volume.CompareTo(Volume.MinValue) < 0 || volume.CompareTo(Volume.MaxValue) > 0)
            {
                throw new ArgumentException($"{usage ?? "Volume"} must be between {Volume.MinValue.Value} and {Volume.MaxValue.Value}.", parameterName ?? nameof(volume));
            }
        }
    }
}
