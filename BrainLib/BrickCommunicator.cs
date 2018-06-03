using Lego.Ev3.Core;
using Lego.Ev3.Core.Enums;
using Lego.Ev3.Core.Events;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BrainLib
{
    public sealed class BrickCommunicator : IDisposable
    {
        private readonly Brick _brick;
        private readonly NotifyMethodAsync _notify;

        public BrickCommunicator(Brick brick, NotifyMethodAsync notify)
        {
            _brick = brick;
            _notify = notify;
        }

        public async Task ConnectAsync()
        {
            await _brick.ConnectAsync(ConnectionType.Usb);
            await _brick.DirectCommandFactory.OutputReadyAsync(OutputPort.All);
            await _brick.DirectCommandFactory.StopAllAsync();
        }

        public async Task ExecuteAsync<TAction>(Func<Task> onCompleted)
            where TAction : AbstractAction
        {
            await ActionExecutor.ExecuteAsync<TAction>(_brick, _notify);

            await onCompleted();
        }

        public void Dispose()
        {
            _brick.Disconnect();
            _brick.Dispose();
        }
    }
}
