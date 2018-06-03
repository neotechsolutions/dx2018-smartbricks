// <copyright file="DummyCommunicationFactory.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Communication
{
    using System;
    using Lego.Ev3.Core.Enums;
    using Lego.Ev3.Core.Interfaces;

    /// <summary>
    /// Dummy object for testing. Return always a <see cref="DummyCommunication"/> instance.
    /// </summary>
    public sealed class DummyCommunicationFactory : ICommunicationFactory
    {
        /// <inheritdoc />
        public ICommunication CreateCommunication(ConnectionType connectionType, string connectionParameter)
        {
            return new DummyCommunication();
        }
    }
}
