using BrainLib;
using Lego.Ev3.Core;
using Lego.Ev3.Core.Exceptions;
using Microsoft.Azure.Devices.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrainLib
{
    public delegate Task NotifyMethodAsync(string methodName, string message);

    public sealed class DeviceListener : IDisposable
    {
        private const string DeviceConnectionString = "HostName=[<IoT Hub URL>];DeviceId=[<Device Id>};SharedAccessKey=[<Shared Access Key>]";

        private readonly DeviceClient _deviceClient;
        private readonly BrickCommunicator _communicator;

        private readonly NotifyMethodAsync _notify;

        public DeviceListener(Brick brick, NotifyMethodAsync notify)
        {
            _deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Mqtt);
            _communicator = new BrickCommunicator(brick, notify);
            _notify = notify ?? new NotifyMethodAsync((method, msg) => Task.CompletedTask);
        }

        public void Dispose()
        {
            _deviceClient.SetMethodHandlerAsync("init", null, null);
            _deviceClient.SetMethodHandlerAsync("loadbag", null, null);
            _deviceClient.SetMethodHandlerAsync("close", null, null);

            _communicator.Dispose();
        }

        public async Task ConnectAsync(CancellationToken token)
        {
            await _notify("ConnectAsync", "Registering direct method");

            await _deviceClient.SetMethodHandlerAsync("init", InitArm, null);
            await _deviceClient.SetMethodHandlerAsync("loadbag", LoadBag, null);
            await _deviceClient.SetMethodHandlerAsync("close", Close, null);

            await _notify("ConnectAsync", "Register direct method done");

            try
            {
                await _notify("ConnectAsync", "Connecting to EV3...");

                await _communicator.ConnectAsync();

                await _notify("ConnectAsync", "Connected to EV3");

                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    await Task.Delay(500);
                }
            }
            catch (CommunicationErrorException ex)
            {
                await _notify("ConnectAsync", $"Error ({ex.Message})");
            }
        }

        public async Task InitializeAsync()
        {
            await _notify("InitializeAsync", "Initialize EV3");

            await _communicator.ExecuteAsync<InitAction>(() => _notify("StartAsync", "Initialize EV3 completed"));
        }

        public async Task LoadBagAsync(bool init = false)
        {
            if (init)
            {
                await InitializeAsync();
            }

            await _notify("LoadBagAsync", "Execute method");

            _communicator.ExecuteAsync<LoadBagAction>(async () => await NotifyCompletedAsync("loaded")).GetAwaiter();
        }

        public async Task CloseAsync(bool init = false)
        {
            await _notify("CloseAsync", "Execute method");

            _communicator.ExecuteAsync<CloseAction>(async () => await NotifyCompletedAsync("closed")).GetAwaiter();
        }

        private Task<MethodResponse> InitArm(MethodRequest methodRequest, object userContext)
        {
            InitializeAsync().GetAwaiter();

            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes("{\"status\":\"initialized\"}"), 200));
        }

        private Task<MethodResponse> LoadBag(MethodRequest methodRequest, object userContext)
        {
            LoadBagAsync(true).GetAwaiter();

            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes("{\"status\":\"loading\"}"), 200));
        }

        private Task<MethodResponse> Close(MethodRequest methodRequest, object userContext)
        {
            CloseAsync().GetAwaiter();

            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes("{\"status\":\"closing\"}"), 200));
        }

        private async Task NotifyCompletedAsync(string data)
        {
            await _notify("NotifyCompletedAsync", $"Send message to IoT Hub with data '{data}'");

            var message = new Message(Encoding.ASCII.GetBytes(data));

            await _deviceClient.SendEventAsync(message);
        }
    }
}
