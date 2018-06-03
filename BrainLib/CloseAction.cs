using Lego.Ev3.Core;
using System;
using System.Threading.Tasks;

namespace BrainLib
{
    public class CloseAction : AbstractAction
    {
        public CloseAction(Brick brick, NotifyMethodAsync notify)
            : base(brick, notify)
        {
        }

        public override async Task ExecuteAsync()
        {
            await Notify("ExecuteAsync", "Rotate to door");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.C);
            await Brick.DirectCommandFactory.StepMotorAtPowerAsync(OutputPort.C, -25, 150, true);
            await Task.Delay(2000);

            await Notify("ExecuteAsync", "Lower arm");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.B);
            await Brick.DirectCommandFactory.StepMotorAtSpeedAsync(OutputPort.B, 10, 60, true);
            await Task.Delay(2000);

            await Notify("ExecuteAsync", "Rotate to start position");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.C);
            await Brick.DirectCommandFactory.StepMotorAtPowerAsync(OutputPort.C, 50, 150, true);
            await Task.Delay(2000);
        }
    }
}
