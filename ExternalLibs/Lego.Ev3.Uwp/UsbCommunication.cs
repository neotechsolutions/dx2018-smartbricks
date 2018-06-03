// <copyright file="UsbCommunication.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Uwp
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading.Tasks;
    using Lego.Ev3.Core.Communication;
    using Lego.Ev3.Core.Enums;
    using Windows.Devices.Enumeration;
    using Windows.Devices.HumanInterfaceDevice;
    using Windows.Storage;

    /// <summary>
    /// Communicate with EV3 brick over USB HID
    /// </summary>
    public sealed class UsbCommunication : CommunicationBase
    {
        private const ushort VID = 0x0694;
        private const ushort PID = 0x0005;
        private const ushort UsagePage = 0xff00;
        private const ushort UsageId = 0x0001;

        private HidDevice _hidDevice;

        /// <inheritdoc/>
        protected override ConnectionType ConnectionType => ConnectionType.Usb;

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
            if (_hidDevice != null)
            {
                _hidDevice.InputReportReceived -= OnHidDeviceInputReportReceived;
                _hidDevice.Dispose();
                _hidDevice = null;
            }
        }

        /// <summary>
        /// Write data to the EV3 brick.
        /// </summary>
        /// <param name="data">Byte array to write to the EV3 brick.</param>
        /// <returns>A task.</returns>
        public override Task WriteAsync([ReadOnlyArray]byte[] data)
        {
            return WriteAsyncInternal(data);
        }

        private async Task ConnectAsyncInternal()
        {
            string selector = HidDevice.GetDeviceSelector(UsagePage, UsageId, VID, PID);
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);
            DeviceInformation brick = devices.FirstOrDefault();
            if (brick == null)
            {
                ThrowException("No LEGO EV3 bricks found.");
            }

            _hidDevice = await HidDevice.FromIdAsync(brick.Id, FileAccessMode.ReadWrite);
            if (_hidDevice == null)
            {
                ThrowException("Unable to connect to LEGO EV3 brick... Is the manifest set properly ?");
            }

            _hidDevice.InputReportReceived += OnHidDeviceInputReportReceived;
        }

        private async Task WriteAsyncInternal(byte[] data)
        {
            if (_hidDevice == null)
            {
                return;
            }

            HidOutputReport report = _hidDevice.CreateOutputReport();
            data.CopyTo(0, report.Data, 1, data.Length);
            await _hidDevice.SendOutputReportAsync(report);
        }

        private void OnHidDeviceInputReportReceived(HidDevice sender, HidInputReportReceivedEventArgs args)
        {
            byte[] data = args.Report.Data.ToArray();

            short size = (short)(data[1] | data[2] << 8);
            if (size == 0)
            {
                return;
            }

            byte[] receivedData = new byte[size];
            Array.Copy(data, 3, receivedData, 0, size);
            RaiseDataReceived(receivedData);
        }
    }
}
