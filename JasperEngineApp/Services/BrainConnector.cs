using Microsoft.Azure.Devices;
using Microsoft.Azure.EventHubs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JasperEngineApp.Services
{
    public static class BrainConnector
    {
		private const string IoTHubConnectionString = "HostName=[<IoT Hub URL>];DeviceId=[<Device Id>};SharedAccessKey=[<Shared Access Key>]";
        private const string EventHubConnectionString = "Endpoint=[<IoT Hub Event Hub Endpoint URL>]/;SharedAccessKeyName=[<Shared Access Key Name>];SharedAccessKey=[<Shared Access Key>];EntityPath=[<Event Hub Name>]";
        private const string DeviceId = "[<Device Id>]";

        private static readonly ServiceClient _serviceClient;
        private static readonly EventHubClient _eventHubClient;

        private static ManualResetEventSlim _eventReceived;
        private static CancellationTokenSource _tokenSource;
        private static List<Task> _receiveTasks;

        static BrainConnector()
        {
            _serviceClient = ServiceClient.CreateFromConnectionString(IoTHubConnectionString);
            _eventHubClient = EventHubClient.CreateFromConnectionString(EventHubConnectionString);

            _eventReceived = new ManualResetEventSlim(false);
            _tokenSource = new CancellationTokenSource();
            _receiveTasks = new List<Task>();
        }

        public static async Task<(int status, string payload)> LoadBagAsync()
        {
            Task.Run(async () => await StartReceiveMessagesAsync());

            CloudToDeviceMethod method = new CloudToDeviceMethod("loadbag");
            method.ResponseTimeout = TimeSpan.FromSeconds(30);

            var methodResult = await _serviceClient.InvokeDeviceMethodAsync(DeviceId, method);
            return (methodResult.Status, methodResult.GetPayloadAsJson());
        }

        public static async Task WaitUntilBagLoadedAsync()
        {
            _eventReceived.Wait();

            _tokenSource.Cancel();

            await Task.WhenAll(_receiveTasks.ToArray());

            _tokenSource.Dispose();

            _receiveTasks.Clear();
        }

        private static async Task StartReceiveMessagesAsync()
        {
            _eventReceived.Reset();

            var runtimeInfo = await _eventHubClient.GetRuntimeInformationAsync();
            var d2cPartitions = runtimeInfo.PartitionIds;

            _tokenSource = new CancellationTokenSource();

            foreach (string partition in d2cPartitions)
            {
                _receiveTasks.Add(ReceiveMessagesFromDeviceAsync(partition, _tokenSource.Token));
            }
        }

        private static async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
        {
            var eventHubReceiver = _eventHubClient.CreateReceiver("$Default", partition, EventPosition.FromEnqueuedTime(DateTime.Now));

            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                var events = await eventHubReceiver.ReceiveAsync(100);
                if (events == null)
                {
                    continue;
                }

                foreach (EventData eventData in events)
                {
                    string data = Encoding.UTF8.GetString(eventData.Body.Array);
                    if (data == "loaded")
                    {
                        _eventReceived.Set();
                        break;
                    }
                }
            }
        }
    }
}
