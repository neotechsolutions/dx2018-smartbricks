// <copyright file="ICommunicationFactory.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Interfaces
{
    using Lego.Ev3.Core.Enums;

    /// <summary>
    /// Factory for instances of class allowing communication with the EV3 brick
    /// </summary>
    public interface ICommunicationFactory
    {
        /// <summary>
        /// Creates the instance of communication for the specified connection type.
        /// </summary>
        /// <param name="connectionType">Type of the connection.</param>
        /// <param name="connectionParameter">The parameter for the creation.</param>
        /// <returns>
        /// An instance of <see cref="ICommunication" />.
        /// </returns>
        ICommunication CreateCommunication(ConnectionType connectionType, string connectionParameter);
    }
}
