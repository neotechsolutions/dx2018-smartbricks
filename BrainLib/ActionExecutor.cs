using System;
using System.Threading.Tasks;
using Lego.Ev3.Core;

namespace BrainLib
{
    public static class ActionExecutor
    {
        public static Task ExecuteAsync<TAction>(Brick brick, NotifyMethodAsync notify)
            where TAction : AbstractAction
        {
            var ctor = typeof(TAction).GetConstructor(new Type[] { typeof(Brick), typeof(NotifyMethodAsync) });
            var action = ctor.Invoke(new object[] { brick, notify }) as AbstractAction;
            return action.ExecuteAsync();
        }
    }
}
