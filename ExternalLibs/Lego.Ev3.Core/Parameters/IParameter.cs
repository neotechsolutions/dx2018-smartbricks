// <copyright file="IParameter.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Parameters
{
    using System;

    /// <summary>
    /// Describe a parameter to be used in a command.
    /// </summary>
    /// <typeparam name="T">Type of the parameter.</typeparam>
    public interface IParameter<T> : IComparable, IComparable<T>, IEquatable<T>
        where T : struct
    {
    }
}
