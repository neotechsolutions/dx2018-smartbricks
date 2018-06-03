// <copyright file="Brick.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Lego.Ev3.Core.Commands;
    using Lego.Ev3.Core.Enums;
    using Lego.Ev3.Core.Events;
    using Lego.Ev3.Core.Extensions;
    using Lego.Ev3.Core.Interfaces;

    /// <summary>
    /// Main EV3 brick interface
    /// </summary>
    public sealed class Brick : IDisposable
    {
        private static readonly TimeSpan DefaultPollingTime = TimeSpan.FromMilliseconds(100);

        private readonly SynchronizationContext _context = SynchronizationContext.Current;
        private readonly ICommunicationFactory _communicationFactory;
        private readonly bool _alwaysSendEvents;

        private CancellationTokenSource _tokenSource;
        private ICommunication _comm;

        /// <summary>
        /// Initializes a new instance of the <see cref="Brick"/> class.
        /// </summary>
        /// <param name="communicationFactory">Object implementing the <see cref="ICommunicationFactory"/> interface.</param>
        /// <param name="fileProvider">Object implementing the <see cref="IFileProvider"/> interface.</param>
        /// <param name="alwaysSendEvents">Send events when data changes, or at every poll</param>
        public Brick(ICommunicationFactory communicationFactory, IFileProvider fileProvider, bool alwaysSendEvents = false)
        {
            DirectCommandFactory = new DirectCommandFactory(this);
            SystemCommandFactory = new SystemCommandFactory(this, fileProvider);
            BatchCommandFactory = new BatchCommandFactory(this);

            Buttons = new BrickButtons();

            _communicationFactory = communicationFactory;
            _alwaysSendEvents = alwaysSendEvents;

            int index = 0;

            Ports = new Dictionary<InputPort, Port>();

            foreach (InputPort i in Enum.GetValues(typeof(InputPort)))
            {
                Ports[i] = new Port
                {
                    InputPort = i,
                    Index = index++,
                    Name = i.ToString(),
                };
            }
        }

        /// <summary>
        /// Event that is fired when a port is changed
        /// </summary>
        public event EventHandler<BrickChangedEventArgs> BrickChanged;

        /// <summary>
        /// Gets the input and output ports on LEGO EV3 brick
        /// </summary>
        public IDictionary<InputPort, Port> Ports { get; }

        /// <summary>
        /// Gets the buttons on the face of the LEGO EV3 brick
        /// </summary>
        public BrickButtons Buttons { get; }

        /// <summary>
        /// Gets the factory to send "direct commands" to the EV3 brick.  These commands are executed instantly and are not batched.
        /// </summary>
        public DirectCommandFactory DirectCommandFactory { get; }

        /// <summary>
        /// Gets the factory to send "system commands" to the EV3 brick.  These commands are executed instantly and are not batched.
        /// </summary>
        public SystemCommandFactory SystemCommandFactory { get; }

        /// <summary>
        /// Gets the factory to send a batch command of multiple direct commands at once.  Call the <see cref="BatchCommandFactory.Initialize(CommandType)"/> method with the proper <see cref="CommandType"/> to set the type of command the batch should be executed as.
        /// </summary>
        public BatchCommandFactory BatchCommandFactory { get; }

        /// <summary>
        /// Connect to the EV3 brick using the specified connection type.
        /// </summary>
        /// <param name="connectionType">The type of connection with the EV3 brick.</param>
        /// <param name="connectionParameter">The connection parameter or <c>null</c>.</param>
        /// <returns>A task.</returns>
        public Task ConnectAsync(ConnectionType connectionType, string connectionParameter = null)
        {
            return ConnectAsync(connectionType, connectionParameter, DefaultPollingTime);
        }

        /// <summary>
        /// Connect to the EV3 brick using the specified connection type and a specified polling time.
        /// </summary>
        /// <param name="connectionType">The type of connection with the EV3 brick.</param>
        /// <param name="connectionParameter">The connection parameter.</param>
        /// <param name="pollingTime">The period to poll the device status.  Set to TimeSpan.Zero to disable polling.</param>
        /// <returns>A task.</returns>
        public Task ConnectAsync(ConnectionType connectionType, string connectionParameter, TimeSpan pollingTime)
        {
            if (_comm != null)
            {
                throw new InvalidOperationException("Brick still connected, must be disconnected");
            }

            _comm = _communicationFactory.CreateCommunication(connectionType, connectionParameter);
            _comm.DataReceived += OnDataReceived;

            return ConnectAsyncInternal(pollingTime);
        }

        /// <summary>
        /// Disconnect from the EV3 brick
        /// </summary>
        public void Disconnect()
        {
            _tokenSource?.Cancel();

            _comm.DataReceived -= OnDataReceived;
            _comm.Disconnect();
            _comm = null;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Disconnect();

            if (_tokenSource != null)
            {
                _tokenSource.Dispose();
                _tokenSource = null;
            }
        }

        internal async Task SendCommandAsyncInternal(Command c)
        {
            await _comm.WriteAsync(c.ToBytes());
            if (c.CommandType == CommandType.DirectReply || c.CommandType == CommandType.SystemReply)
            {
                await ResponseManager.WaitForResponseAsync(c.Response);
            }
        }

        private async Task ConnectAsyncInternal(TimeSpan pollingTime)
        {
            _tokenSource = new CancellationTokenSource();

            await _comm.ConnectAsync();

            await DirectCommandFactory.StopAllAsync();

            if (pollingTime != TimeSpan.Zero)
            {
                Task.Factory.StartNew(
                        async () =>
                        {
                            while (!_tokenSource.IsCancellationRequested)
                            {
                                await PollSensorsAsync();
                                await Task.Delay(pollingTime, _tokenSource.Token);
                            }

                            await DirectCommandFactory.StopAllAsync();
                        },
                        _tokenSource.Token,
                        TaskCreationOptions.LongRunning,
                        TaskScheduler.Current).GetAwaiter();
            }
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            ResponseManager.HandleResponse(e.Data);
        }

        private async Task PollSensorsAsync()
        {
            bool changed = false;
            const int responseSize = 11;
            int index = 0;

            Command c = new Command(CommandType.DirectReply, (8 * responseSize) + 6, 0);

            foreach (InputPort i in Enum.GetValues(typeof(InputPort)))
            {
                Port p = Ports[i];
                index = p.Index * responseSize;

                c.GetTypeMode(p.InputPort, (byte)index, (byte)(index + 1));
                c.ReadySI(p.InputPort, p.Mode, (byte)(index + 2));
                c.ReadyRaw(p.InputPort, p.Mode, (byte)(index + 6));
                c.ReadyPercent(p.InputPort, p.Mode, (byte)(index + 10));
            }

            index += responseSize;

            c.IsBrickButtonPressed(BrickButton.Back, (byte)(index + 0));
            c.IsBrickButtonPressed(BrickButton.Left, (byte)(index + 1));
            c.IsBrickButtonPressed(BrickButton.Up, (byte)(index + 2));
            c.IsBrickButtonPressed(BrickButton.Right, (byte)(index + 3));
            c.IsBrickButtonPressed(BrickButton.Down, (byte)(index + 4));
            c.IsBrickButtonPressed(BrickButton.Enter, (byte)(index + 5));

            await SendCommandAsyncInternal(c);
            if (c.Response.Data == null)
            {
                return;
            }

            foreach (InputPort i in Enum.GetValues(typeof(InputPort)))
            {
                Port p = Ports[i];

                int type = c.Response.Data[(p.Index * responseSize) + 0];
                byte mode = c.Response.Data[(p.Index * responseSize) + 1];
                float siValue = BitConverter.ToSingle(c.Response.Data, (p.Index * responseSize) + 2);
                int rawValue = BitConverter.ToInt32(c.Response.Data, (p.Index * responseSize) + 6);
                byte percentValue = c.Response.Data[(p.Index * responseSize) + 10];

                if ((byte)p.Type != type || Math.Abs(p.SIValue - siValue) > 0.01f || p.RawValue != rawValue || p.PercentValue != percentValue)
                {
                    changed = true;
                }

                if (Enum.IsDefined(typeof(DeviceType), type))
                {
                    p.Type = (DeviceType)type;
                }
                else
                {
                    p.Type = DeviceType.Unknown;
                }

                p.SIValue = siValue;
                p.RawValue = rawValue;
                p.PercentValue = percentValue;
            }

            if (Buttons.Back != (c.Response.Data[index + 0] == 1) ||
                Buttons.Left != (c.Response.Data[index + 1] == 1) ||
                Buttons.Up != (c.Response.Data[index + 2] == 1) ||
                Buttons.Right != (c.Response.Data[index + 3] == 1) ||
                Buttons.Down != (c.Response.Data[index + 4] == 1) ||
                Buttons.Enter != (c.Response.Data[index + 5] == 1))
            {
                changed = true;
            }

            Buttons.Back = c.Response.Data[index + 0] == 1;
            Buttons.Left = c.Response.Data[index + 1] == 1;
            Buttons.Up = c.Response.Data[index + 2] == 1;
            Buttons.Right = c.Response.Data[index + 3] == 1;
            Buttons.Down = c.Response.Data[index + 4] == 1;
            Buttons.Enter = c.Response.Data[index + 5] == 1;

            if (changed || _alwaysSendEvents)
            {
                OnBrickChanged(new BrickChangedEventArgs(Ports, Buttons));
            }
        }

        private void OnBrickChanged(BrickChangedEventArgs e)
        {
            EventHandler<BrickChangedEventArgs> handler = BrickChanged;
            if (handler != null)
            {
                if (_context == SynchronizationContext.Current)
                {
                    handler(this, e);
                }
                else
                {
                    _context.Post(delegate { handler(this, e); }, null);
                }
            }
        }
    }
}
