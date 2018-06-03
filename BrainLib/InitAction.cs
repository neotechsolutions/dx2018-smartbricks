using Lego.Ev3.Core;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BrainLib
{
    public class InitAction : AbstractAction
    {
        public InitAction(Brick brick, NotifyMethodAsync notify)
            : base(brick, notify)
        {
        }

        public override async Task ExecuteAsync()
        {
            if (await Brick.DirectCommandFactory.ReadySIAsync(InputPort.Three, (int)ColorMode.Reflective) < Constants.MaxColorReflexionSIValue)
            {
                await Notify("ExecuteAsync", "Initialize arm vertical position");

                await Brick.DirectCommandFactory.TurnMotorAtPowerAsync(OutputPort.B, -25);

                await Brick.WaitUntilAsync(InputPort.Three, p => p.SIValue >= Constants.MaxColorReflexionSIValue);

                await Brick.DirectCommandFactory.StopMotorAsync(OutputPort.B, true);
                await Task.Delay(1000);
            }

            await Notify("ExecuteAsync", "Check arm hand");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.A);
            await Brick.DirectCommandFactory.TurnMotorAtPowerForTimeAsync(OutputPort.A, 30, 1000, true);
            await Task.Delay(1000);

            await Notify("ExecuteAsync", "Lower arm before rotation");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.B);
            await Brick.DirectCommandFactory.TurnMotorAtSpeedForTimeAsync(OutputPort.B, 15, 600, true);
            await Task.Delay(2000);

            if (await Brick.DirectCommandFactory.ReadySIAsync(InputPort.One, (int)TouchMode.Touch) != 1.0)
            {
                await Notify("ExecuteAsync", "Initialize arm horizontal position");

                await Brick.DirectCommandFactory.TurnMotorAtPowerAsync(OutputPort.C, 30);

                await Brick.WaitUntilAsync(InputPort.One, p => p.SIValue == 1);

                await Brick.DirectCommandFactory.StopMotorAsync(OutputPort.C, true);
                await Task.Delay(1000);
            }

            await Notify("ExecuteAsync", "Rotate at start position");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.C);
            await Brick.DirectCommandFactory.StepMotorAtPowerAsync(OutputPort.C, -20, 350, true);
            await Task.Delay(5000);
        }
    }
}