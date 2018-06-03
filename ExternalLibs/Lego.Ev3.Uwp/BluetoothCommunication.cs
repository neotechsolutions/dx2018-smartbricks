// <copyright file="BluetoothCommunication.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Uwp
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading;
    using System.Threading.Tasks;
    using Lego.Ev3.Core.Communication;
    using Lego.Ev3.Core.Enums;
    using Windows.Devices.Bluetooth.Rfcomm;
    using Windows.Devices.Enumeration;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;

    using WindowThreadPool = Windows.System.Threading.ThreadPool;

    /// <summary>
    /// Communicate with EV3 brick over Bluetooth.
    /// </summary>
    public sealed class BluetoothCommunication : CommunicationBase
    {
        private const string DefaultDeviceName = "EV3";

        private readonly string _deviceName;

        private StreamSocket _socket;
        private DataReader _reader;
        private CancellationTokenSource _tokenSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothCommunication"/> class.
        /// </summary>
        /// <param name="device">Device name of the EV3 brick</param>
        public BluetoothCommunication(string device)
        {
            _deviceName = device ?? DefaultDeviceName;
        }

        /// <inheritdoc/>
        protected override ConnectionType ConnectionType => ConnectionType.Bluetooth;

        /// <summary>
        /// Connect to the EV3 brick.
        /// </summary>
        /// <returns>A task.</returns>
        public override Task ConnectAsync()
        {
            return ConnectAsyncInternal();
        }

        /// <summary>
        /// Disconnect from the EV3 brick.
        /// </summary>
        public override void Disconnect()
        {
            _tokenSource.Cancel();
            if (_reader != null)
            {
                _reader.DetachStream();
                _reader = null;
            }

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
        public override async Task WriteAsync([ReadOnlyArray]byte[] data)
        {
            if (_socket != null)
            {
                await _socket.OutputStream.WriteAsync(data.AsBuffer());
            }
        }

        private async Task ConnectAsyncInternal()
        {
            _tokenSource = new CancellationTokenSource();

            string selector = RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort);
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);
            DeviceInformation device = (from d in devices where d.Name == _deviceName select d).FirstOrDefault();
            if (device == null)
            {
                ThrowException($"LEGO EV3 brick named '{_deviceName}' not found.");
            }

            RfcommDeviceService service = await RfcommDeviceService.FromIdAsync(device.Id);
            if (service == null)
            {
                ThrowException("Unable to connect to LEGO EV3 brick... Is the manifest set properly ?");
            }

            _socket = new StreamSocket();
            await _socket.ConnectAsync(
                service.ConnectionHostName,
                service.ConnectionServiceName,
                SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);

            _reader = new DataReader(_socket.InputStream)
            {
                ByteOrder = ByteOrder.LittleEndian
            };

            await WindowThreadPool.RunAsync(async (_) => await PollInputAsync());
        }

        private async Task PollInputAsync()
        {
            while (_socket != null)
            {
                try
                {
                    DataReaderLoadOperation drlo = _reader.LoadAsync(2);
                    await drlo.AsTask(_tokenSource.Token);
                    short size = _reader.ReadInt16();
                    byte[] data = new byte[size];

                    drlo = _reader.LoadAsync((uint)size);
                    await drlo.AsTask(_tokenSource.Token);
                    _reader.ReadBytes(data);

                    RaiseDataReceived(data);
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }
    }
}
