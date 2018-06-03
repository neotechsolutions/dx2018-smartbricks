// <copyright file="CommandExtensions.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Extensions
{
    using System;
    using Lego.Ev3.Core.Commands;
    using Lego.Ev3.Core.Helpers;
    using Lego.Ev3.Core.Parameters;
    using Lego.Ev3.Core.Parameters.Display;

    /// <summary>
    /// Extension class for the <see cref="Command"/>
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Start the motor(s) based on previous commands
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">Port or ports to apply the command to.</param>
        /// <returns>The updated command.</returns>
        public static Command StartMotor(this Command c, OutputPort ports)
        {
            c.AddOpcode(Opcode.OutputStart);
            c.AddParameter(0x00);         // layer
            c.AddParameter((byte)ports);  // ports

            return c;
        }

        /// <summary>
        /// Turns the specified motor at the specified power
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">Port or ports to apply the command to.</param>
        /// <param name="power">The amount of power to apply to the specified motors (-100% to 100%).</param>
        /// <returns>The updated command.</returns>
        public static Command TurnMotorAtPower(this Command c, OutputPort ports, Power power)
        {
            ParameterHelper.VerifyPowerInRange(power);

            c.AddOpcode(Opcode.OutputPower);
            c.AddParameter(0x00);         // layer
            c.AddParameter((byte)ports);  // ports
            c.AddParameter((sbyte)power);  // power

            return c;
        }

        /// <summary>
        /// Turn the specified motor at the specified speed.
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">Port or ports to apply the command to.</param>
        /// <param name="speed">The speed to apply to the specified motors (-100% to 100%).</param>
        /// <returns>The updated command.</returns>
        public static Command TurnMotorAtSpeed(this Command c, OutputPort ports, Speed speed)
        {
            ParameterHelper.VerifySpeedInRange(speed);

            c.AddOpcode(Opcode.OutputSpeed);
            c.AddParameter(0x00);         // layer
            c.AddParameter((byte)ports);  // ports
            c.AddParameter((sbyte)speed);      // speed

            return c;
        }

        /// <summary>
        /// Step the motor connected to the specified port or ports at the specified power for the specified number of steps.
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="power">The power at which to turn the motor (-100% to 100%).</param>
        /// <param name="steps">The number of steps to turn the motor.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>The updated command.</returns>
        public static Command StepMotorAtPower(this Command c, OutputPort ports, Power power, uint steps, bool brake)
        {
            return c.StepMotorAtPower(ports, power, 0, steps, 10, brake);
        }

        /// <summary>
        /// Step the motor connected to the specified port or ports at the specified power for the specified number of steps.
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="power">The power at which to turn the motor (-100% to 100%).</param>
        /// <param name="rampUpSteps">Number of steps to get up to power.</param>
        /// <param name="constantSteps">Number of steps to run at constant power.</param>
        /// <param name="rampDownSteps">Number of steps to power down to a stop.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>The updated command.</returns>
        public static Command StepMotorAtPower(this Command c, OutputPort ports, Power power, uint rampUpSteps, uint constantSteps, uint rampDownSteps, bool brake)
        {
            ParameterHelper.VerifyPowerInRange(power);

            c.AddOpcode(Opcode.OutputStepPower);
            c.AddParameter(0x00);         // layer
            c.AddParameter((byte)ports);  // ports
            c.AddParameter((sbyte)power);          // power
            c.AddParameter(rampUpSteps);  // step1
            c.AddParameter(constantSteps);    // step2
            c.AddParameter(rampDownSteps);    // step3
            c.AddParameter((byte)(brake ? 0x01 : 0x00));      // brake (0 = coast, 1 = brake)

            return c;
        }

        /// <summary>
        /// Step the motor connected to the specified port or ports at the specified speed for the specified number of steps.
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="speed">The speed at which to turn the motor (-100% to 100%).</param>
        /// <param name="steps">The number of steps to turn the motor.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>The updated command.</returns>
        public static Command StepMotorAtSpeed(this Command c, OutputPort ports, Speed speed, uint steps, bool brake)
        {
            return c.StepMotorAtSpeed(ports, speed, 0, steps, 10, brake);
        }

        /// <summary>
        /// Step the motor connected to the specified port or ports at the specified speed for the specified number of steps.
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="speed">The speed at which to turn the motor (-100% to 100%).</param>
        /// <param name="rampUpSteps">Number of steps to get up to speed.</param>
        /// <param name="constantSteps">Number of steps to run at constant speed.</param>
        /// <param name="rampDownSteps">Number of steps to speed down to a stop.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>The updated command.</returns>
        public static Command StepMotorAtSpeed(this Command c, OutputPort ports, Speed speed, uint rampUpSteps, uint constantSteps, uint rampDownSteps, bool brake)
        {
            ParameterHelper.VerifySpeedInRange(speed);

            c.AddOpcode(Opcode.OutputStepSpeed);
            c.AddParameter(0x00);         // layer
            c.AddParameter((byte)ports);  // ports
            c.AddParameter((sbyte)speed);          // speed
            c.AddParameter(rampUpSteps);  // step1
            c.AddParameter(constantSteps);    // step2
            c.AddParameter(rampDownSteps);    // step3
            c.AddParameter((byte)(brake ? 0x01 : 0x00));      // brake (0 = coast, 1 = brake)

            return c;
        }

        /// <summary>
        /// Turn the motor connected to the specified port or ports at the specified power for the specified times.
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="power">The power at which to turn the motor (-100% to 100%).</param>
        /// <param name="milliseconds">Number of milliseconds to run at constant power.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>The updated command.</returns>
        public static Command TurnMotorAtPowerForTime(this Command c, OutputPort ports, Power power, uint milliseconds, bool brake)
        {
            return c.TurnMotorAtPowerForTime(ports, power, 0, milliseconds, 0, brake);
        }

        /// <summary>
        /// Turn the motor connected to the specified port or ports at the specified power for the specified times.
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="power">The power at which to turn the motor (-100% to 100%).</param>
        /// <param name="msRampUp">Number of milliseconds to get up to power.</param>
        /// <param name="msConstant">Number of milliseconds to run at constant power.</param>
        /// <param name="msRampDown">Number of milliseconds to power down to a stop.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>The updated command.</returns>
        public static Command TurnMotorAtPowerForTime(this Command c, OutputPort ports, Power power, uint msRampUp, uint msConstant, uint msRampDown, bool brake)
        {
            ParameterHelper.VerifyPowerInRange(power);

            c.AddOpcode(Opcode.OutputTimePower);
            c.AddParameter(0x00);         // layer
            c.AddParameter((byte)ports);  // ports
            c.AddParameter((sbyte)power);  // power
            c.AddParameter(msRampUp);     // step1
            c.AddParameter(msConstant);   // step2
            c.AddParameter(msRampDown);   // step3
            c.AddParameter((byte)(brake ? 0x01 : 0x00));      // brake (0 = coast, 1 = brake)

            return c;
        }

        /// <summary>
        /// Turn the motor connected to the specified port or ports at the specified speed for the specified times.
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="speed">The speed at which to turn the motor (-100 to 100).</param>
        /// <param name="milliseconds">Number of milliseconds to run at constant speed.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>The updated command.</returns>
        public static Command TurnMotorAtSpeedForTime(this Command c, OutputPort ports, Speed speed, uint milliseconds, bool brake)
        {
            return c.TurnMotorAtSpeedForTime(ports, speed, 0, milliseconds, 0, brake);
        }

        /// <summary>
        /// Turn the motor connected to the specified port or ports at the specified speed for the specified times.
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">A specific port or Ports.All.</param>
        /// <param name="speed">The speed at which to turn the motor (-100 to 100).</param>
        /// <param name="msRampUp">Number of milliseconds to get up to speed.</param>
        /// <param name="msConstant">Number of milliseconds to run at constant speed.</param>
        /// <param name="msRampDown">Number of milliseconds to slow down to a stop.</param>
        /// <param name="brake">Apply brake to motor at end of routine.</param>
        /// <returns>The updated command.</returns>
        public static Command TurnMotorAtSpeedForTime(this Command c, OutputPort ports, Speed speed, uint msRampUp, uint msConstant, uint msRampDown, bool brake)
        {
            ParameterHelper.VerifySpeedInRange(speed);

            c.AddOpcode(Opcode.OutputTimeSpeed);
            c.AddParameter(0x00);         // layer
            c.AddParameter((byte)ports);  // ports
            c.AddParameter((sbyte)speed);          // power
            c.AddParameter(msRampUp);     // step1
            c.AddParameter(msConstant);       // step2
            c.AddParameter(msRampDown);       // step3
            c.AddParameter((byte)(brake ? 0x01 : 0x00));      // brake (0 = coast, 1 = brake)

            return c;
        }

        /// <summary>
        /// Append the Set Polarity command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">Port or ports to change polarity</param>
        /// <param name="polarity">The new polarity (direction) value</param>
        /// <returns>The updated command.</returns>
        public static Command SetMotorPolarity(this Command c, OutputPort ports, Polarity polarity)
        {
            c.AddOpcode(Opcode.OutputPolarity);
            c.AddParameter(0x00);
            c.AddParameter((byte)ports);
            c.AddParameter((byte)polarity);

            return c;
        }

        /// <summary>
        /// Synchronize stepping of motors.
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">The port or ports to which the stop command will be sent.</param>
        /// <param name="speed">Speed to turn the motor(s). (-100 to 100)</param>
        /// <param name="turnRatio">The turn ratio to apply. (-200 to 200)</param>
        /// <param name="step">The number of steps to turn the motor(s).</param>
        /// <param name="brake">Brake or coast at the end.</param>
        /// <returns>The updated command.</returns>
        public static Command StepMotorSync(this Command c, OutputPort ports, Speed speed, TurnRatio turnRatio, uint step, bool brake)
        {
            ParameterHelper.VerifySpeedInRange(speed);
            ParameterHelper.VerifyTurnRatioInRange(turnRatio);

            c.AddOpcode(Opcode.OutputStepSync);
            c.AddParameter(0x00);
            c.AddParameter((byte)ports);
            c.AddParameter((sbyte)speed);
            c.AddParameter(turnRatio);
            c.AddParameter(step);
            c.AddParameter((byte)(brake ? 0x01 : 0x00));      // brake (0 = coast, 1 = brake)

            return c;
        }

        /// <summary>
        /// Synchronize timing of motors.
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">The port or ports to which the stop command will be sent.</param>
        /// <param name="speed">Speed to turn the motor(s). (-100 to 100)</param>
        /// <param name="turnRatio">The turn ratio to apply. (-200 to 200)</param>
        /// <param name="time">The time to turn the motor(s).</param>
        /// <param name="brake">Brake or coast at the end.</param>
        /// <returns>The updated command.</returns>
        public static Command TimeMotorSync(this Command c, OutputPort ports, Speed speed, TurnRatio turnRatio, uint time, bool brake)
        {
            ParameterHelper.VerifySpeedInRange(speed);
            ParameterHelper.VerifyTurnRatioInRange(turnRatio);

            c.AddOpcode(Opcode.OutputTimeSync);
            c.AddParameter(0x00);
            c.AddParameter((byte)ports);
            c.AddParameter((sbyte)speed);
            c.AddParameter(turnRatio);
            c.AddParameter(time);
            c.AddParameter((byte)(brake ? 0x01 : 0x00));      // brake (0 = coast, 1 = brake)

            return c;
        }

        /// <summary>
        /// Append the Stop Motor command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">Port or ports to stop</param>
        /// <param name="brake">Apply the brake at the end of the command</param>
        /// <returns>The updated command.</returns>
        public static Command StopMotor(this Command c, OutputPort ports, bool brake)
        {
            c.AddOpcode(Opcode.OutputStop);
            c.AddParameter(0x00);         // layer
            c.AddParameter((byte)ports);  // ports
            c.AddParameter((byte)(brake ? 0x01 : 0x00));      // brake (0 = coast, 1 = brake)

            return c;
        }

        /// <summary>
        /// Append the Stop Motor command to an existing Command object for all outputs.
        /// </summary>
        /// <param name="c">The command.</param>
        /// <returns>The updated command.</returns>
        public static Command StopAll(this Command c)
        {
            c.AddOpcode(Opcode.OutputStop);
            c.AddParameter(0x00);         // layer
            c.AddParameter((byte)OutputPort.All);  // ports
            c.AddParameter((byte)0x00);      // brake (0 = coast)

            return c;
        }

        /// <summary>
        /// Append the Clear All Devices command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <returns>The updated command.</returns>
        public static Command ClearAllDevices(this Command c)
        {
            c.AddOpcode(Opcode.InputDevice_ClearAll);
            c.AddParameter(0x00);         // layer

            return c;
        }

        /// <summary>
        /// Append the Clear Changes command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="port">Port to clear changes.</param>
        /// <returns>The updated command.</returns>
        public static Command ClearChanges(this Command c, InputPort port)
        {
            c.AddOpcode(Opcode.InputDevice_ClearChanges);
            c.AddParameter(0x00);         // layer
            c.AddParameter((byte)port);           // port

            return c;
        }

        /// <summary>
        /// Append the Play Tone command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="volume">Volume to play the tone (0-100)</param>
        /// <param name="frequency">Frequency of the tone in hertz</param>
        /// <param name="duration">Duration of the tone in milliseconds</param>
        /// <returns>The updated command.</returns>
        public static Command PlayTone(this Command c, Volume volume, ushort frequency, ushort duration)
        {
            ParameterHelper.VerifyVolumeInRange(volume);

            c.AddOpcode(Opcode.Sound_Tone);
            c.AddParameter((byte)volume);     // volume
            c.AddParameter(frequency);    // frequency
            c.AddParameter(duration); // duration (ms)

            return c;
        }

        /// <summary>
        /// Append the Play Sound command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="volume">Volume to play the sound</param>
        /// <param name="filename">Filename on the Brick of the sound to play</param>
        /// <returns>The updated command.</returns>
        public static Command PlaySound(this Command c, Volume volume, string filename)
        {
            ParameterHelper.VerifyVolumeInRange(volume);

            c.AddOpcode(Opcode.Sound_Play);
            c.AddParameter((byte)volume);
            c.AddParameter(filename);

            return c;
        }

        /// <summary>
        /// Append the Get Firmware Version command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="maxLength">Maximum length of string to be returned</param>
        /// <param name="index">Index at which the data should be returned inside of the global buffer</param>
        /// <returns>The updated command.</returns>
        public static Command GetFirwmareVersion(this Command c, uint maxLength, Index index)
        {
            if (maxLength > 0xff)
            {
                throw new ArgumentException("String length cannot be greater than 255 bytes", nameof(maxLength));
            }

            ParameterHelper.VerifyIndexValid(index);

            c.AddOpcode(Opcode.UIRead_GetFirmware);
            c.AddParameter((byte)maxLength);      // global buffer size
            c.AddGlobalIndex((byte)index);        // index where buffer begins

            return c;
        }

        /// <summary>
        /// Add the Is Brick Pressed command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="button">Button to check</param>
        /// <param name="index">Index at which the data should be returned inside of the global buffer</param>
        /// <returns>The updated command.</returns>
        public static Command IsBrickButtonPressed(this Command c, BrickButton button, Index index)
        {
            ParameterHelper.VerifyIndexValid(index);

            c.AddOpcode(Opcode.UIButton_Pressed);
            c.AddParameter((byte)button);
            c.AddGlobalIndex((byte)index);

            return c;
        }

        /// <summary>
        /// Append the Set LED Pattern command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ledPattern">The LED pattern to display</param>
        /// <returns>The updated command.</returns>
        public static Command SetLedPattern(this Command c, LedPattern ledPattern)
        {
            c.AddOpcode(Opcode.UIWrite_LED);
            c.AddParameter((byte)ledPattern);

            return c;
        }

        /// <summary>
        /// Append the Clean UI command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <returns>The updated command.</returns>
        public static Command CleanUI(this Command c)
        {
            c.AddOpcode(Opcode.UIDraw_Clean);

            return c;
        }

        /// <summary>
        /// Append the Draw Line command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="color">Color of the line</param>
        /// <param name="x0">X start</param>
        /// <param name="y0">Y start</param>
        /// <param name="x1">X end</param>
        /// <param name="y1">Y end</param>
        /// <returns>The updated command.</returns>
        public static Command DrawLine(this Command c, Color color, ushort x0, ushort y0, ushort x1, ushort y1)
        {
            c.AddOpcode(Opcode.UIDraw_Line);
            c.AddParameter((byte)color);
            c.AddParameter(x0);
            c.AddParameter(y0);
            c.AddParameter(x1);
            c.AddParameter(y1);

            return c;
        }

        /// <summary>
        /// Append the Draw Pixel command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="color">Color of the pixel</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>The updated command.</returns>
        public static Command DrawPixel(this Command c, Color color, ushort x, ushort y)
        {
            c.AddOpcode(Opcode.UIDraw_Pixel);
            c.AddParameter((byte)color);
            c.AddParameter(x);
            c.AddParameter(y);

            return c;
        }

        /// <summary>
        /// Append the Draw Rectangle command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="color">Color of the rectangle</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        /// <param name="filled">Draw a filled or empty rectangle</param>
        /// <returns>The updated command.</returns>
        public static Command DrawRectangle(this Command c, Color color, ushort x, ushort y, ushort width, ushort height, bool filled)
        {
            c.AddOpcode(filled ? Opcode.UIDraw_FillRect : Opcode.UIDraw_Rect);
            c.AddParameter((byte)color);
            c.AddParameter(x);
            c.AddParameter(y);
            c.AddParameter(width);
            c.AddParameter(height);

            return c;
        }

        /// <summary>
        /// Append the Draw Inverse Rectangle command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        /// <returns>The updated command.</returns>
        public static Command DrawInverseRectangle(this Command c, ushort x, ushort y, ushort width, ushort height)
        {
            c.AddOpcode(Opcode.UIDraw_InverseRect);
            c.AddParameter(x);
            c.AddParameter(y);
            c.AddParameter(width);
            c.AddParameter(height);

            return c;
        }

        /// <summary>
        /// Append the Draw Circle command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="color">Color of the circle</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="radius">Radius of circle</param>
        /// <param name="filled">Draw a filled or empty circle</param>
        /// <returns>The updated command.</returns>
        public static Command DrawCircle(this Command c, Color color, ushort x, ushort y, ushort radius, bool filled)
        {
            c.AddOpcode(filled ? Opcode.UIDraw_FillCircle : Opcode.UIDraw_Circle);
            c.AddParameter((byte)color);
            c.AddParameter(x);
            c.AddParameter(y);
            c.AddParameter(radius);

            return c;
        }

        /// <summary>
        /// Append the Draw Text command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="color">Color of the text</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="text">Text to draw</param>
        /// <returns>The updated command.</returns>
        public static Command DrawText(this Command c, Color color, ushort x, ushort y, string text)
        {
            c.AddOpcode(Opcode.UIDraw_Text);
            c.AddParameter((byte)color);
            c.AddParameter(x);
            c.AddParameter(y);
            c.AddParameter(text);

            return c;
        }

        /// <summary>
        /// Append the Draw Fill Window command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="color">The color to fill</param>
        /// <param name="y0">Y start</param>
        /// <param name="y1">Y end</param>
        /// <returns>The updated command.</returns>
        public static Command DrawFillWindow(this Command c, Color color, ushort y0, ushort y1)
        {
            c.AddOpcode(Opcode.UIDraw_FillWindow);
            c.AddParameter((byte)color);
            c.AddParameter(y0);
            c.AddParameter(y1);

            return c;
        }

        /// <summary>
        /// Append the Draw Image command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="color">The color of the image to draw</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="devicePath">Filename on the brick of the image to draw</param>
        /// <returns>The updated command.</returns>
        public static Command DrawImage(this Command c, Color color, ushort x, ushort y, string devicePath)
        {
            c.AddOpcode(Opcode.UIDraw_BmpFile);
            c.AddParameter((byte)color);
            c.AddParameter(x);
            c.AddParameter(y);
            c.AddParameter(devicePath);

            return c;
        }

        /// <summary>
        /// Append the Select Font command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="fontType">The font to select</param>
        /// <returns>The updated command.</returns>
        public static Command SelectFont(this Command c, FontType fontType)
        {
            c.AddOpcode(Opcode.UIDraw_SelectFont);
            c.AddParameter((byte)fontType);

            return c;
        }

        /// <summary>
        /// Append the Enable Top Line command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="enabled">Enable/disable the top status bar line</param>
        /// <returns>The updated command.</returns>
        public static Command EnableTopLine(this Command c, bool enabled)
        {
            c.AddOpcode(Opcode.UIDraw_Topline);
            c.AddParameter((byte)(enabled ? 1 : 0));

            return c;
        }

        /// <summary>
        /// Append the Draw Dotted Line command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="color">The color of the line</param>
        /// <param name="x0">X start</param>
        /// <param name="y0">Y start</param>
        /// <param name="x1">X end</param>
        /// <param name="y1">Y end</param>
        /// <param name="onPixels">Number of pixels the line is on</param>
        /// <param name="offPixels">Number of pixels the line is off</param>
        /// <returns>The updated command.</returns>
        public static Command DrawDottedLine(this Command c, Color color, ushort x0, ushort y0, ushort x1, ushort y1, ushort onPixels, ushort offPixels)
        {
            c.AddOpcode(Opcode.UIDraw_DotLine);
            c.AddParameter((byte)color);
            c.AddParameter(x0);
            c.AddParameter(y0);
            c.AddParameter(x1);
            c.AddParameter(y1);
            c.AddParameter(onPixels);
            c.AddParameter(offPixels);

            return c;
        }

        /// <summary>
        /// Append the Update UI command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <returns>The updated command.</returns>
        public static Command UpdateUI(this Command c)
        {
            c.AddOpcode(Opcode.UIDraw_Update);

            return c;
        }

        /// <summary>
        /// Append the Delete File command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="devicePath">Filename on the brick to delete</param>
        /// <returns>The updated command.</returns>
        public static Command DeleteFile(this Command c, string devicePath)
        {
            c.AddOpcode(SystemOpcode.DeleteFile);
            c.AddRawParameter(devicePath);

            return c;
        }

        /// <summary>
        /// Append the Create Directory command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="devicePath">Directory name on the brick to create</param>
        /// <returns>The updated command.</returns>
        public static Command CreateDirectory(this Command c, string devicePath)
        {
            c.AddOpcode(SystemOpcode.CreateDirectory);
            c.AddRawParameter(devicePath);

            return c;
        }

        /// <summary>
        /// Append the Get Type/Mode command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="port">The port to query</param>
        /// <param name="typeIndex">The index to hold the Type value in the global buffer</param>
        /// <param name="modeIndex">The index to hold the Mode value in the global buffer</param>
        /// <returns>The updated command.</returns>
        public static Command GetTypeMode(this Command c, InputPort port, Index typeIndex, Index modeIndex)
        {
            ParameterHelper.VerifyIndexValid(typeIndex, nameof(typeIndex), "Index for Type");
            ParameterHelper.VerifyIndexValid(modeIndex, nameof(modeIndex), "Index for Mode");

            c.AddOpcode(Opcode.InputDevice_GetTypeMode);
            c.AddParameter(0x00);         // layer
            c.AddParameter((byte)port);   // port
            c.AddGlobalIndex((byte)typeIndex);    // index for type
            c.AddGlobalIndex((byte)modeIndex);    // index for mode

            return c;
        }

        /// <summary>
        /// Append the Ready SI command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="port">The port to query</param>
        /// <param name="mode">The mode to read the data as</param>
        /// <param name="index">The index to hold the return value in the global buffer</param>
        /// <returns>The updated command.</returns>
        public static Command ReadySI(this Command c, InputPort port, int mode, Index index)
        {
            ParameterHelper.VerifyIndexValid(index);

            c.AddOpcode(Opcode.InputDevice_ReadySI);
            c.AddParameter(0x00);             // layer
            c.AddParameter((byte)port);       // port
            c.AddParameter(0x00);             // type
            c.AddParameter((byte)mode);               // mode
            c.AddParameter(0x01);             // # values
            c.AddGlobalIndex((byte)index);            // index for return data

            return c;
        }

        /// <summary>
        /// Append the Ready Raw command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="port">The port to query</param>
        /// <param name="mode">The mode to query the value as</param>
        /// <param name="index">The index in the global buffer to hold the return value</param>
        /// <returns>The updated command.</returns>
        public static Command ReadyRaw(this Command c, InputPort port, int mode, Index index)
        {
            ParameterHelper.VerifyIndexValid(index);

            c.AddOpcode(Opcode.InputDevice_ReadyRaw);
            c.AddParameter(0x00);             // layer
            c.AddParameter((byte)port);       // port
            c.AddParameter(0x00);             // type
            c.AddParameter((byte)mode);               // mode
            c.AddParameter(0x01);             // # values
            c.AddGlobalIndex((byte)index);            // index for return data

            return c;
        }

        /// <summary>
        /// Append the Ready Percent command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="port">The port to query</param>
        /// <param name="mode">The mode to query the value as</param>
        /// <param name="index">The index in the global buffer to hold the return value</param>
        /// <returns>The updated command.</returns>
        public static Command ReadyPercent(this Command c, InputPort port, int mode, Index index)
        {
            ParameterHelper.VerifyIndexValid(index);

            c.AddOpcode(Opcode.InputDevice_ReadyPct);
            c.AddParameter(0x00);             // layer
            c.AddParameter((byte)port);       // port
            c.AddParameter(0x00);             // type
            c.AddParameter((byte)mode);               // mode
            c.AddParameter(0x01);             // # values
            c.AddGlobalIndex((byte)index);            // index for return data

            return c;
        }

        /// <summary>
        /// Append the Get Device Name command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="port">The port to query</param>
        /// <param name="bufferSize">Size of the buffer to hold the returned data</param>
        /// <param name="index">Index to the position of the returned data in the global buffer</param>
        /// <returns>The updated command.</returns>
        public static Command GetDeviceName(this Command c, InputPort port, int bufferSize, Index index)
        {
            ParameterHelper.VerifyIndexValid(index);

            c.AddOpcode(Opcode.InputDevice_GetDeviceName);
            c.AddParameter(0x00);
            c.AddParameter((byte)port);
            c.AddParameter((byte)bufferSize);
            c.AddGlobalIndex((byte)index);

            return c;
        }

        /// <summary>
        /// Append the Get Mode Name command to an existing Command object
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="port">The port to query</param>
        /// <param name="mode">The mode of the name to get</param>
        /// <param name="bufferSize">Size of the buffer to hold the returned data</param>
        /// <param name="index">Index to the position of the returned data in the global buffer</param>
        /// <returns>The updated command.</returns>
        public static Command GetModeName(this Command c, InputPort port, int mode, int bufferSize, Index index)
        {
            ParameterHelper.VerifyIndexValid(index);

            c.AddOpcode(Opcode.InputDevice_GetModeName);
            c.AddParameter(0x00);
            c.AddParameter((byte)port);
            c.AddParameter((byte)mode);
            c.AddParameter((byte)bufferSize);
            c.AddGlobalIndex((byte)index);

            return c;
        }

        /// <summary>
        /// Wait for the specified output port(s) to be ready for the next command
        /// </summary>
        /// <param name="c">The command.</param>
        /// <param name="ports">Port(s) to wait for</param>
        /// <returns>The updated command.</returns>
        public static Command OutputReady(this Command c, OutputPort ports)
        {
            c.AddOpcode(Opcode.OutputReady);
            c.AddParameter(0x00);         // layer
            c.AddParameter((byte)ports);  // ports

            return c;
        }
    }
}
