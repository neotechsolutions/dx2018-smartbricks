
using System.Threading.Tasks;
using Lego.Ev3.Core;

namespace BrainLib
{
    public class LoadBagAction : AbstractAction
    {
        public LoadBagAction(Brick brick, NotifyMethodAsync notify)
            : base(brick, notify)
        {
        }

        public override async Task ExecuteAsync()
        {
            await Notify("ExecuteAsync", "Open arm hand");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.A);
            await Brick.DirectCommandFactory.StepMotorAtPowerAsync(OutputPort.A, -50, 90, true);
            await Task.Delay(2000);

            if (await Brick.DirectCommandFactory.ReadySIAsync(InputPort.One, (int)TouchMode.Touch) != 1.0)
            {
                await Notify("ExecuteAsync", "Rotate to bag");

                await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.C);
                await Brick.DirectCommandFactory.TurnMotorAtPowerAsync(OutputPort.C, 30);

                await Brick.WaitUntilAsync(InputPort.One, p => p.SIValue == 1);

                await Brick.DirectCommandFactory.StopMotorAsync(OutputPort.C, true);
                await Task.Delay(1000);
            }

            await Notify("ExecuteAsync", "Lower arm");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.B);
            await Brick.DirectCommandFactory.StepMotorAtSpeedAsync(OutputPort.B, 10, 58, true);
            await Task.Delay(1000);

            await Notify("ExecuteAsync", "Take bag with arm");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.A);
            await Brick.DirectCommandFactory.StepMotorAtPowerAsync(OutputPort.A, 50, 90, true);
            await Task.Delay(1000);

            await Notify("ExecuteAsync", "Rise arm");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.B);
            await Brick.DirectCommandFactory.StepMotorAtSpeedAsync(OutputPort.B, -10, 60, true);
            await Task.Delay(2000);

            await Notify("ExecuteAsync", "Rotate to car");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.C);
            await Brick.DirectCommandFactory.StepMotorAtPowerAsync(OutputPort.C, -20, 180, true);
            await Task.Delay(2000);

            await Notify("ExecuteAsync", "End rise arm");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.B);
            await Brick.DirectCommandFactory.StepMotorAtSpeedAsync(OutputPort.B, -10, 30, true);
            await Task.Delay(2000);

            await Notify("ExecuteAsync", "End rotate to car");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.C);
            await Brick.DirectCommandFactory.StepMotorAtPowerAsync(OutputPort.C, -20, 180, true);
            await Task.Delay(4000);

            await Notify("ExecuteAsync", "Lower arm");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.B);
            await Brick.DirectCommandFactory.StepMotorAtSpeedAsync(OutputPort.B, 10, 65, true);
            await Task.Delay(2000);

            await Notify("ExecuteAsync", "Free bag from arm");

            await Brick.DirectCommandFactory.OutputReadyAsync(OutputPort.A);
            await Brick.DirectCommandFactory.StepMotorAtPowerAsync(OutputPort.A, -50, 45, true);
            await Task.Delay(1000);

            if (await Brick.DirectCommandFactory.ReadySIAsync(InputPort.Three, (int)ColorMode.Reflective) < Constants.MaxColorReflexionSIValue)
            {
                await Notify("ExecuteAsync", "Rise arm");

                await Brick.DirectCommandFactory.TurnMotorAtPowerAsync(OutputPort.B, -25);

                await Brick.WaitUntilAsync(InputPort.Three, p => p.SIValue >= Constants.MaxColorReflexionSIValue);

                await Brick.DirectCommandFactory.StopMotorAsync(OutputPort.B, true);
                await Task.Delay(1000);
            }
        }
    }
}
