using Lego.Ev3.Core;
using System.Threading.Tasks;

namespace BrainLib
{
    public abstract class AbstractAction
    {
        protected AbstractAction(Brick brick, NotifyMethodAsync notify)
        {
            Brick = brick;
            Notify = notify;
        }

        protected Brick Brick { get; }

        protected NotifyMethodAsync Notify { get; }

        public abstract Task ExecuteAsync();
    }
}
