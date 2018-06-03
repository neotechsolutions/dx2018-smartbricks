﻿// <copyright file="NetworkCommunication.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Uwp
{
    using System;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Lego.Ev3.Core.Communication;
    using Lego.Ev3.Core.Enums;
    using Windows.Networking;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;
    using Buffer = Windows.Storage.Streams.Buffer;
    using WindowThreadPool = Windows.System.Threading.ThreadPool;

    /// <summary>
    /// Communicate with EV3 brick over TCP
    /// </summary>
    public sealed class NetworkCommunication : CommunicationBase
    {
        private const string UnlockCommand = "GET /target?sn=\r\nProtocol:EV3\r\n\r\n";

        private readonly HostName _hostName;

        private CancellationTokenSource _tokenSource;

        private StreamSocket _socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkCommunication"/> class.
        /// </summary>
        /// <param name="address">The IP address of the EV3 brick</param>
        public NetworkCommunication(string address)
        {
            _hostName = new HostName(address);
        }

        /// <inheritdoc/>
        protected override ConnectionType ConnectionType => ConnectionType.Network;

        /// <summary>
        /// Connect to the EV3 brick.
        /// </summary>
        /// <returns>A task.</returns>
        public override Task ConnectAsync()
        {
            return ConnectAsyncInternal(_hostName);
        }

        /// <summary>
        /// Disconnect from the EV3 brick.
        /// </summary>
        public override void Disconnect()
        {
            _tokenSource.Cancel();

            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }
        }

        /// <summary>
        /// Write data to the EV3 brick.
        /// </summary>
        /// <param name="data">Byte array to write to the EV3 brick.</param>
        /// <returns>A task.</returns>
        public override Task WriteAsync([ReadOnlyArray]byte[] data)
        {
            return _socket.OutputStream.WriteAsync(data.AsBuffer()).AsTask();
        }

        private async Task ConnectAsyncInternal(HostName hostName)
        {
            _tokenSource = new CancellationTokenSource();

            _socket = new StreamSocket();

            // connect to the brick on port 5555
            await _socket.ConnectAsync(hostName, "5555", SocketProtectionLevel.PlainSocket);

            // unlock the brick (doesn't actually need serial number?)
            await _socket.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(UnlockCommand).AsBuffer());

            // read the "Accept:EV340\r\n\r\n" response
            IBuffer bufferResponse = new Buffer(128);
            await _socket.InputStream.ReadAsync(bufferResponse, bufferResponse.Capacity, InputStreamOptions.Partial);
            string response = Encoding.UTF8.GetString(bufferResponse.ToArray(), 0, (int)bufferResponse.Length);
            if (string.IsNullOrEmpty(response))
            {
                ThrowException("LEGO EV3 brick did not respond to the unlock command.");
            }

            await WindowThreadPool.RunAsync(async (_) => await PollInputAsync());
        }

        private async Task PollInputAsync()
        {
            while (!_tokenSource.IsCancellationRequested)
            {
                try
                {
                    IBuffer sizeBuffer = new Buffer(2);
                    await _socket.InputStream.ReadAsync(sizeBuffer, 2, InputStreamOptions.None);
                    uint size = (uint)(sizeBuffer.GetByte(0) | sizeBuffer.GetByte(1) << 8);

                    if (size != 0)
                    {
                        IBuffer data = new Buffer(size);
                        await _socket.InputStream.ReadAsync(data, size, InputStreamOptions.None);
                        RaiseDataReceived(data.ToArray());
                    }
                }
                catch (TaskCanceledException)
                {
                    return;
                }
                catch (Exception)
                {
                    // swallow exceptions...if we tank here, it's likely a disconnect and we can't do much anyway
                }
            }
        }
    }
}
