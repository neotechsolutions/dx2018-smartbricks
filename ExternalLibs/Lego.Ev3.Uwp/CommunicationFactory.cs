// <copyright file="CommunicationFactory.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Uwp
{
    using System;
    using Lego.Ev3.Core.Enums;
    using Lego.Ev3.Core.Interfaces;

    /// <summary>
    /// Implementation of the <see cref="ICommunicationFactory"/> interface for UWP.
    /// </summary>
    public class CommunicationFactory : ICommunicationFactory
    {
        /// <inheritdoc />
        public ICommunication CreateCommunication(ConnectionType connectionType, string connectionParameter)
        {
            ICommunication result = null;

            switch (connectionType)
            {
                case ConnectionType.Bluetooth:
                    if (string.IsNullOrEmpty(connectionParameter))
                    {
                        throw new InvalidOperationException("Unable to create BluetoothCommunication : no COM port provided as parameter");
                    }

                    result = new BluetoothCommunication(connectionParameter);
                    break;
                case ConnectionType.Network:
                    if (string.IsNullOrEmpty(connectionParameter))
                    {
                        throw new InvalidOperationException("Unable to create NetworkCommunication : no IP address provided as parameter");
                    }

                    result = new NetworkCommunication(connectionParameter);
                    break;
                case ConnectionType.Usb:
                    result = new UsbCommunication();
                    break;
                default:
                    throw new NotSupportedException($"{typeof(ConnectionType).Name}.{connectionType} not supported");
            }

            return result;
        }
    }
}
