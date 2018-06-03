using BrainLib;
using Lego.Ev3.Core;
using Lego.Ev3.Uwp;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

using ThreadPool = Windows.System.Threading.ThreadPool;

namespace BrainApp
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private DeviceListener _listener;
        private CancellationTokenSource _source;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnTaskCanceled;

            _source = new CancellationTokenSource();
            _listener = new DeviceListener(
                brick: new Brick(new CommunicationFactory(), new FileProvider()),
                notify: NotifyMethodAsync
            );

            await ThreadPool.RunAsync(async _ =>
            {
                await _listener.ConnectAsync(_source.Token);
                await _listener.InitializeAsync();
            });
        }

        private Task NotifyMethodAsync(string method, string msg)
        {
            Debug.WriteLine($"[DEBUG] {method} : {msg}");
            return Task.CompletedTask;
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (_deferral != null)
            {
                _source.Cancel();
                _listener.Dispose();

                _deferral.Complete();
                _deferral = null;
            }
        }
    }
}
