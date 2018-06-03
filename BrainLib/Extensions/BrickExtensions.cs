using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Lego.Ev3.Core.Events;

namespace Lego.Ev3.Core
{
    public static class BrickExtensions
    {
        public static Task<Port> WaitUntilAsync(this Brick source, InputPort port, Predicate<Port> predicate)
        {
            var tcs = new TaskCompletionSource<Port>();
            EventHandler<BrickChangedEventArgs> handler = null;
            handler = (s, e) =>
            {
#if DEBUG
                Debug.WriteLine("Brick changed...");
                LogPortInfo(e.Ports[InputPort.A]);
                LogPortInfo(e.Ports[InputPort.B]);
                LogPortInfo(e.Ports[InputPort.C]);
                LogPortInfo(e.Ports[InputPort.D]);
                LogPortInfo(e.Ports[InputPort.One]);
                LogPortInfo(e.Ports[InputPort.Two]);
                LogPortInfo(e.Ports[InputPort.Three]);
                LogPortInfo(e.Ports[InputPort.Four]);
#endif

                var result = e.Ports[port];
                if (predicate(result))
                {
                    tcs.SetResult(result);
                    source.BrickChanged -= handler;
                }
            };

            source.BrickChanged += handler;

            return tcs.Task;
        }

#if DEBUG
        private static void LogPortInfo(Port port)
        {
            Debug.WriteLine($"Port {port.Name} of type {port.Type} with mode {port.Mode} : SI={port.SIValue}, Raw={port.RawValue}, Percent={port.PercentValue}");
        }
#endif
    }
}
