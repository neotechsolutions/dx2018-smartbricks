// <copyright file="DummyCommunication.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Communication
{
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;
    using Lego.Ev3.Core.Enums;

    /// <summary>
    /// Dummy object for testing.  Does not actually connect or communicate with EV3 brick.
    /// </summary>
    public sealed class DummyCommunication : CommunicationBase
    {
        /// <summary>
        /// Gets the type of the connection.
        /// </summary>
        protected override ConnectionType ConnectionType => ConnectionType.Unknown;

        /// <summary>
        /// Test Connect method.
        /// </summary>
        /// <returns>A task.</returns>
        public override Task ConnectAsync()
        {
            return Task.Run(() => Debug.WriteLine("connected"));
        }

        /// <summary>
        /// Test Disconnect method.
        /// </summary>
        public override void Disconnect()
        {
        }

        /// <summary>
        /// Test WriteAsync method.  (Writes formatted data to Debug stream).
        /// </summary>
        /// <param name="data">Byte array to be written.</param>
        /// <returns>A task.</returns>
        public override Task WriteAsync(byte[] data)
        {
            return Task.Run(() =>
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 3; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("X2")).Append(" ");
                }

                Debug.WriteLine("Write: " + sb.ToString());
            });
        }
    }
}
