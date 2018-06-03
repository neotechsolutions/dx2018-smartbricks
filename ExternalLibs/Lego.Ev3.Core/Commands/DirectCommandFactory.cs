// <copyright file="DirectCommandFactory.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Commands
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Lego.Ev3.Core.Extensions;

    /// <summary>
    /// Factory for direct commands for the EV3 brick
    /// </summary>
    public sealed class DirectCommandFactory : CommandFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectCommandFactory"/> class.
        /// </summary>
        /// <param name="brick">The brick.</param>
        internal DirectCommandFactory(Brick brick)
            : base(brick)
        {
        }

        /// <summary>
        /// Turn the motor connected to the specified port or ports at the specified power.
        /// </summary>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="power">The power at which to turn the motor (-100 to 100).</param>
        /// <returns>A task.</returns>
        public Task TurnMotorAtPowerAsync(OutputPort ports, int power)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.TurnMotorAtPower(ports, power);
                c.StartMotor(ports);
            });
        }

        /// <summary>
        /// Turn the specified motor at the specified speed.
        /// </summary>
        /// <param name="ports">Port or ports to apply the command to.</param>
        /// <param name="speed">The speed to apply to the specified motors (-100 to 100).</param>
        /// <returns>A task.</returns>
        public Task TurnMotorAtSpeedAsync(OutputPort ports, int speed)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.TurnMotorAtSpeed(ports, speed);
                c.StartMotor(ports);
            });
        }

        /// <summary>
        /// Step the motor connected to the specified port or ports at the specified power for the specified number of steps.
        /// </summary>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="power">The power at which to turn the motor (-100 to 100).</param>
        /// <param name="steps">The steps.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>A task.</returns>
        public Task StepMotorAtPowerAsync(OutputPort ports, int power, uint steps, bool brake)
        {
            return StepMotorAtPowerAsyncInternal(ports, power, 0, steps, 0, brake);
        }

        /// <summary>
        /// Step the motor connected to the specified port or ports at the specified power for the specified number of steps.
        /// </summary>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="power">The power at which to turn the motor (-100 to 100).</param>
        /// <param name="rampUpSteps">The ramp up steps.</param>
        /// <param name="constantSteps">The steps.</param>
        /// <param name="rampDownSteps">The ramp down steps.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>A task.</returns>
        public Task StepMotorAtPowerAsync(OutputPort ports, int power, uint rampUpSteps, uint constantSteps, uint rampDownSteps, bool brake)
        {
            return StepMotorAtPowerAsyncInternal(ports, power, rampUpSteps, constantSteps, rampDownSteps, brake);
        }

        /// <summary>
        /// Step the motor connected to the specified port or ports at the specified speed for the specified number of steps.
        /// </summary>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="speed">The speed at which to turn the motor (-100 to 100).</param>
        /// <param name="steps">The steps.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>A task.</returns>
        public Task StepMotorAtSpeedAsync(OutputPort ports, int speed, uint steps, bool brake)
        {
            return StepMotorAtSpeedAsyncInternal(ports, speed, 0, steps, 0, brake);
        }

        /// <summary>
        /// Step the motor connected to the specified port or ports at the specified speed for the specified number of steps.
        /// </summary>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="speed">The speed at which to turn the motor (-100 to 100).</param>
        /// <param name="rampUpSteps">The ramp up steps.</param>
        /// <param name="constantSteps">The steps.</param>
        /// <param name="rampDownSteps">The ramp down steps.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>A task.</returns>
        public Task StepMotorAtSpeedAsync(OutputPort ports, int speed, uint rampUpSteps, uint constantSteps, uint rampDownSteps, bool brake)
        {
            return StepMotorAtSpeedAsyncInternal(ports, speed, rampUpSteps, constantSteps, rampDownSteps, brake);
        }

        /// <summary>
        /// Turn the motor connected to the specified port or ports at the specified power for the specified times.
        /// </summary>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="power">The power at which to turn the motor (-100 to 100).</param>
        /// <param name="milliseconds">Number of milliseconds to run at constant power.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>A task.</returns>
        public Task TurnMotorAtPowerForTimeAsync(OutputPort ports, int power, uint milliseconds, bool brake)
        {
            return TurnMotorAtPowerForTimeAsyncInternal(ports, power, 0, milliseconds, 0, brake);
        }

        /// <summary>
        /// Turn the motor connected to the specified port or ports at the specified power for the specified times.
        /// </summary>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="power">The power at which to turn the motor (-100 to 100).</param>
        /// <param name="msRampUp">Number of milliseconds to get up to power.</param>
        /// <param name="msConstant">Number of milliseconds to run at constant power.</param>
        /// <param name="msRampDown">Number of milliseconds to power down to a stop.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>A task.</returns>
        public Task TurnMotorAtPowerForTimeAsync(OutputPort ports, int power, uint msRampUp, uint msConstant, uint msRampDown, bool brake)
        {
            return TurnMotorAtPowerForTimeAsyncInternal(ports, power, msRampUp, msConstant, msRampDown, brake);
        }

        /// <summary>
        /// Turn the motor connected to the specified port or ports at the specified speed for the specified times.
        /// </summary>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="speed">The power at which to turn the motor (-100 to 100).</param>
        /// <param name="milliseconds">Number of milliseconds to run at constant speed.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>A task.</returns>
        public Task TurnMotorAtSpeedForTimeAsync(OutputPort ports, int speed, uint milliseconds, bool brake)
        {
            return TurnMotorAtSpeedForTimeAsyncInternal(ports, speed, 0, milliseconds, 0, brake);
        }

        /// <summary>
        /// Turn the motor connected to the specified port or ports at the specified speed for the specified times.
        /// </summary>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="speed">The power at which to turn the motor (-100 to 100).</param>
        /// <param name="msRampUp">Number of milliseconds to get up to speed.</param>
        /// <param name="msConstant">Number of milliseconds to run at constant speed.</param>
        /// <param name="msRampDown">Number of milliseconds to slow down to a stop.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>A task.</returns>
        public Task TurnMotorAtSpeedForTimeAsync(OutputPort ports, int speed, uint msRampUp, uint msConstant, uint msRampDown, bool brake)
        {
            return TurnMotorAtSpeedForTimeAsyncInternal(ports, speed, msRampDown, msConstant, msRampDown, brake);
        }

        /// <summary>
        /// Set the polarity (direction) of a motor.
        /// </summary>
        /// <param name="ports">Port or ports to change polarity</param>
        /// <param name="polarity">The new polarity (direction) value</param>
        /// <returns>A task.</returns>
        public Task SetMotorPolarityAsync(OutputPort ports, Polarity polarity)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.SetMotorPolarity(ports, polarity);
            });
        }

        /// <summary>
        /// Start motors on the specified ports.
        /// </summary>
        /// <param name="ports">The port or ports to which the stop command will be sent.</param>
        /// <returns>A task.</returns>
        public Task StartMotorAsync(OutputPort ports)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.StartMotor(ports);
            });
        }

        /// <summary>
        /// Synchronize stepping of motors.
        /// </summary>
        /// <param name="ports">The port or ports to which the stop command will be sent.</param>
        /// <param name="speed">Speed to turn the motor(s).</param>
        /// <param name="turnRatio">The turn ratio to apply.</param>
        /// <param name="step">The number of steps to turn the motor(s).</param>
        /// <param name="brake">Brake or coast at the end.</param>
        /// <returns>A task.</returns>
        public Task StepMotorSyncAsync(OutputPort ports, int speed, short turnRatio, uint step, bool brake)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.StepMotorSync(ports, speed, turnRatio, step, brake);
            });
        }

        /// <summary>
        /// Synchronize timing of motors.
        /// </summary>
        /// <param name="ports">The port or ports to which the stop command will be sent.</param>
        /// <param name="speed">Speed to turn the motor(s).</param>
        /// <param name="turnRatio">The turn ratio to apply.</param>
        /// <param name="time">The time to turn the motor(s).</param>
        /// <param name="brake">Brake or coast at the end.</param>
        /// <returns>A task.</returns>
        public Task TimeMotorSyncAsync(OutputPort ports, int speed, short turnRatio, uint time, bool brake)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.TimeMotorSync(ports, speed, turnRatio, time, brake);
            });
        }

        /// <summary>
        /// Stops motors on the specified ports.
        /// </summary>
        /// <param name="ports">The port or ports to which the stop command will be sent.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>A task.</returns>
        public Task StopMotorAsync(OutputPort ports, bool brake)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.StopMotor(ports, brake);
            });
        }

        /// <summary>
        /// Stops all the outputs.
        /// </summary>
        /// <returns>A task.</returns>
        public Task StopAllAsync()
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.StopMotor(OutputPort.All, false);
            });
        }

        /// <summary>
        /// Resets all ports and devices to defaults.
        /// </summary>
        /// <returns>A task.</returns>
        public Task ClearAllDevicesAsync()
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.ClearAllDevices();
            });
        }

        /// <summary>
        /// Clears changes on specified port
        /// </summary>
        /// <param name="port">The port to clear</param>
        /// <returns>A task.</returns>
        public Task ClearChangesAsync(InputPort port)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.ClearChanges(port);
            });
        }

        /// <summary>
        /// Plays a tone of the specified frequency for the specified time.
        /// </summary>
        /// <param name="volume">Volume of tone (0-100).</param>
        /// <param name="frequency">Frequency of tone, in hertz.</param>
        /// <param name="duration">Duration to play tone, in milliseconds.</param>
        /// <returns>A task.</returns>
        public Task PlayToneAsync(int volume, ushort frequency, ushort duration)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.PlayTone(volume, frequency, duration);
            });
        }

        /// <summary>
        /// Play a sound file stored on the EV3 brick
        /// </summary>
        /// <param name="volume">Volume of the sound (0-100)</param>
        /// <param name="filename">Filename of sound stored on brick, without the .RSF extension</param>
        /// <returns>A task.</returns>
        public Task PlaySoundAsync(int volume, string filename)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.PlaySound(volume, filename);
            });
        }

        /// <summary>
        /// Return the current version number of the firmware running on the EV3 brick.
        /// </summary>
        /// <returns>Current firmware version.</returns>
        public Task<string> GetFirmwareVersionAsync()
        {
            return ExecuteDirectWithReplyAsync<string>(0x10, 0, c => c.GetFirwmareVersion(0x10, 0), data =>
            {
                if (data == null)
                {
                    return null;
                }

                int index = Array.IndexOf(data, (byte)0);
                return Encoding.UTF8.GetString(data, 0, index);
            });
        }

        /// <summary>
        /// Returns whether the specified BrickButton is pressed
        /// </summary>
        /// <param name="button">Button on the face of the EV3 brick</param>
        /// <returns>Whether or not the button is pressed</returns>
        public Task<bool> IsBrickButtonPressedAsync(BrickButton button)
        {
            return ExecuteDirectWithReplyAsync<bool>(1, 0, c => c.IsBrickButtonPressed(button, 0), data =>
            {
                return BitConverter.ToBoolean(data, 0);
            });
        }

        /// <summary>
        /// Set EV3 brick LED pattern
        /// </summary>
        /// <param name="ledPattern">Pattern to display on LED</param>
        /// <returns>A task.</returns>
        public Task SetLedPatternAsync(LedPattern ledPattern)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.SetLedPattern(ledPattern);
            });
        }

        /// <summary>
        /// Draw a line on the EV3 LCD screen
        /// </summary>
        /// <param name="color">Color of the line</param>
        /// <param name="x0">X start</param>
        /// <param name="y0">Y start</param>
        /// <param name="x1">X end</param>
        /// <param name="y1">Y end</param>
        /// <returns>A task.</returns>
        public Task DrawLineAsync(Color color, ushort x0, ushort y0, ushort x1, ushort y1)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.DrawLine(color, x0, y0, x1, y1);
            });
        }

        /// <summary>
        /// Draw a single pixel
        /// </summary>
        /// <param name="color">Color of the pixel</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>A task.</returns>
        public Task DrawPixelAsync(Color color, ushort x, ushort y)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.DrawPixel(color, x, y);
            });
        }

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="color">Color of the rectangle</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        /// <param name="filled">Filled or empty</param>
        /// <returns>A task.</returns>
        public Task DrawRectangleAsync(Color color, ushort x, ushort y, ushort width, ushort height, bool filled)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.DrawRectangle(color, x, y, width, height, filled);
            });
        }

        /// <summary>
        /// Draw a filled rectangle, inverting the pixels underneath it
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        /// <returns>A task.</returns>
        public Task DrawInverseRectangleAsync(ushort x, ushort y, ushort width, ushort height)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.DrawInverseRectangle(x, y, width, height);
            });
        }

        /// <summary>
        /// Draw a circle
        /// </summary>
        /// <param name="color">Color of the circle</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="radius">Radius of the circle</param>
        /// <param name="filled">Filled or empty</param>
        /// <returns>A task.</returns>
        public Task DrawCircleAsync(Color color, ushort x, ushort y, ushort radius, bool filled)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.DrawCircle(color, x, y, radius, filled);
            });
        }

        /// <summary>
        /// Write a string to the screen
        /// </summary>
        /// <param name="color">Color of the text</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="text">Text to draw</param>
        /// <returns>A task.</returns>
        public Task DrawTextAsync(Color color, ushort x, ushort y, string text)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.DrawText(color, x, y, text);
            });
        }

        /// <summary>
        /// Draw a dotted line
        /// </summary>
        /// <param name="color">Color of dotted line</param>
        /// <param name="x0">X start</param>
        /// <param name="y0">Y start</param>
        /// <param name="x1">X end</param>
        /// <param name="y1">Y end</param>
        /// <param name="onPixels">Number of pixels the line is drawn</param>
        /// <param name="offPixels">Number of pixels the line is empty</param>
        /// <returns>A task.</returns>
        public Task DrawDottedLineAsync(Color color, ushort x0, ushort y0, ushort x1, ushort y1, ushort onPixels, ushort offPixels)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.DrawDottedLine(color, x0, y0, x1, y1, onPixels, offPixels);
            });
        }

        /// <summary>
        /// Fills the width of the screen between the provided Y coordinates
        /// </summary>
        /// <param name="color">Color of the fill</param>
        /// <param name="y0">Y start</param>
        /// <param name="y1">Y end</param>
        /// <returns>A task.</returns>
        public Task DrawFillWindowAsync(Color color, ushort y0, ushort y1)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.DrawFillWindow(color, y0, y1);
            });
        }

        /// <summary>
        /// Draw an image to the LCD screen
        /// </summary>
        /// <param name="color">Color of the image pixels</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="devicePath">Path to the image on the EV3 brick</param>
        /// <returns>A task.</returns>
        public Task DrawImageAsync(Color color, ushort x, ushort y, string devicePath)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.DrawImage(color, x, y, devicePath);
            });
        }

        /// <summary>
        /// Enable or disable the top status bar
        /// </summary>
        /// <param name="enabled">Enabled or disabled</param>
        /// <returns>A task.</returns>
        public Task EnableTopLineAsync(bool enabled)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.EnableTopLine(enabled);
            });
        }

        /// <summary>
        /// Select the font for text drawing
        /// </summary>
        /// <param name="fontType">Type of font to use</param>
        /// <returns>A task.</returns>
        public Task SelectFontAsync(FontType fontType)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.SelectFont(fontType);
            });
        }

        /// <summary>
        /// Clear the entire screen
        /// </summary>
        /// <returns>A task.</returns>
        public Task CleanUIAsync()
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.CleanUI();
            });
        }

        /// <summary>
        /// Refresh the EV3 LCD screen
        /// </summary>
        /// <returns>A task.</returns>
        public Task UpdateUIAsync()
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.UpdateUI();
            });
        }

        /// <summary>
        /// Get the type and mode of the device attached to the specified port
        /// </summary>
        /// <param name="port">The input port to query</param>
        /// <returns>2 bytes, index 0 being the type, index 1 being the mode</returns>
        public Task<byte[]> GetTypeModeAsync(InputPort port)
        {
            return ExecuteDirectWithReplyAsync<byte[]>(2, 0, c => c.GetTypeMode(port, 0, 1), data => data);
        }

        /// <summary>
        /// Read the SI value from the specified port in the specified mode
        /// </summary>
        /// <param name="port">The port to query</param>
        /// <param name="mode">The mode used to read the data</param>
        /// <returns>The SI value</returns>
        public Task<float> ReadySIAsync(InputPort port, int mode)
        {
            return ExecuteDirectWithReplyAsync<float>(4, 0, c => c.ReadySI(port, mode, 0), data => BitConverter.ToSingle(data, 0));
        }

        /// <summary>
        /// Read the raw value from the specified port in the specified mode
        /// </summary>
        /// <param name="port">The port to query</param>
        /// <param name="mode">The mode used to read the data</param>
        /// <returns>The Raw value</returns>
        public Task<int> ReadyRawAsync(InputPort port, int mode)
        {
            return ExecuteDirectWithReplyAsync<int>(4, 0, c => c.ReadyRaw(port, mode, 0), data => BitConverter.ToInt32(data, 0));
        }

        /// <summary>
        /// Read the percent value from the specified port in the specified mode
        /// </summary>
        /// <param name="port">The port to query</param>
        /// <param name="mode">The mode used to read the data</param>
        /// <returns>The percentage value</returns>
        public Task<int> ReadyPercentAsync(InputPort port, int mode)
        {
            return ExecuteDirectWithReplyAsync<int>(1, 0, c => c.ReadyRaw(port, mode, 0), data => data[0]);
        }

        /// <summary>
        /// Get the name of the device attached to the specified port
        /// </summary>
        /// <param name="port">Port to query</param>
        /// <returns>The name of the device</returns>
        public Task<string> GetDeviceNameAsync(InputPort port)
        {
            return ExecuteDirectWithReplyAsync<string>(0x7f, 0, c => c.GetDeviceName(port, 0x7f, 0), data =>
            {
                int index = Array.IndexOf(data, (byte)0);
                return Encoding.UTF8.GetString(data, 0, index);
            });
        }

        /// <summary>
        /// Get the mode of the device attached to the specified port
        /// </summary>
        /// <param name="port">Port to query</param>
        /// <param name="mode">Mode of the name to get</param>
        /// <returns>The name of the mode</returns>
        public Task<string> GetModeNameAsync(InputPort port, int mode)
        {
            return ExecuteDirectWithReplyAsync<string>(0x7f, 0, c => c.GetModeName(port, mode, 0x7f, 0), data =>
            {
                int index = Array.IndexOf(data, (byte)0);
                return Encoding.UTF8.GetString(data, 0, index);
            });
        }

        /// <summary>
        /// Wait for the specified output port(s) to be ready for the next command
        /// </summary>
        /// <param name="ports">Port(s) to wait for</param>
        /// <returns>A task.</returns>
        public Task OutputReadyAsync(OutputPort ports)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.OutputReady(ports);
            });
        }

        /// <summary>
        /// Internal method allowing to step the motor connected to the specified port or ports at the specified power for the specified number of steps.
        /// </summary>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="power">The power at which to turn the motor (-100 to 100).</param>
        /// <param name="rampUpSteps">The ramp up steps.</param>
        /// <param name="constantSteps">The steps.</param>
        /// <param name="rampDownSteps">The ramp down steps.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>A task.</returns>
        internal Task StepMotorAtPowerAsyncInternal(OutputPort ports, int power, uint rampUpSteps, uint constantSteps, uint rampDownSteps, bool brake)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.StepMotorAtPower(ports, power, rampUpSteps, constantSteps, rampDownSteps, brake);
            });
        }

        /// <summary>
        /// Internal method allowing to step the motor connected to the specified port or ports at the specified speed for the specified number of steps.
        /// </summary>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="speed">The speed at which to turn the motor (-100 to 100).</param>
        /// <param name="rampUpSteps">The ramp up steps.</param>
        /// <param name="constantSteps">The steps.</param>
        /// <param name="rampDownSteps">The ramp down steps.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>A task.</returns>
        internal Task StepMotorAtSpeedAsyncInternal(OutputPort ports, int speed, uint rampUpSteps, uint constantSteps, uint rampDownSteps, bool brake)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.StepMotorAtSpeed(ports, speed, rampUpSteps, constantSteps, rampDownSteps, brake);
            });
        }

        /// <summary>
        /// Internal method allowing to turn the motor connected to the specified port or ports at the specified power for the specified times.
        /// </summary>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="power">The power at which to turn the motor (-100 to 100).</param>
        /// <param name="msRampUp">Number of milliseconds to get up to power.</param>
        /// <param name="msConstant">Number of milliseconds to run at constant power.</param>
        /// <param name="msRampDown">Number of milliseconds to power down to a stop.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>A task.</returns>
        internal Task TurnMotorAtPowerForTimeAsyncInternal(OutputPort ports, int power, uint msRampUp, uint msConstant, uint msRampDown, bool brake)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.TurnMotorAtPowerForTime(ports, power, msRampUp, msConstant, msRampDown, brake);
            });
        }

        /// <summary>
        /// Internal method allowing to turn the motor connected to the specified port or ports at the specified speed for the specified times.
        /// </summary>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="speed">The speed at which to turn the motor (-100 to 100).</param>
        /// <param name="msRampUp">Number of milliseconds to get up to power.</param>
        /// <param name="msConstant">Number of milliseconds to run at constant power.</param>
        /// <param name="msRampDown">Number of milliseconds to power down to a stop.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>A task.</returns>
        internal Task TurnMotorAtSpeedForTimeAsyncInternal(OutputPort ports, int speed, uint msRampUp, uint msConstant, uint msRampDown, bool brake)
        {
            return ExecuteDirectWithoutReplyAsync(c =>
            {
                c.TurnMotorAtSpeedForTime(ports, speed, msRampUp, msConstant, msRampDown, brake);
            });
        }

        /// <summary>
        /// Execute a direct command without a reply.
        /// </summary>
        /// <param name="action">The action to set on the command.</param>
        /// <returns>A task.</returns>
        private async Task ExecuteDirectWithoutReplyAsync(Action<Command> action)
        {
            using (var ctx = CreateContext(CommandType.DirectNoReply))
            {
                action.Invoke(ctx.Command);
                await ctx.ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// Execute a direct command with a reply.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="globalSize">The size of the global buffer in bytes (maximum of 1024 bytes)</param>
        /// <param name="localSize">The size of the local buffer in bytes (maximum of 64 bytes)</param>
        /// <param name="action">The action to set on the command.</param>
        /// <param name="converter">The converter for obtain the result from the response.</param>
        /// <returns>The reply.</returns>
        private async Task<TResult> ExecuteDirectWithReplyAsync<TResult>(ushort globalSize, int localSize, Action<Command> action, Func<byte[], TResult> converter)
        {
            using (var ctx = CreateContext(CommandType.DirectReply, globalSize, localSize))
            {
                action.Invoke(ctx.Command);
                await ctx.ExecuteCommandAsync();

                return converter.Invoke(ctx.Command.Response.Data);
            }
        }
    }
}
