// <copyright file="Command.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Commands
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Command to be send to the EV3 brick
    /// </summary>
    public sealed class Command : IDisposable
    {
        private BinaryWriter _writer;
        private MemoryStream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        internal Command(CommandType commandType)
            : this(commandType, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="globalSize">The size of the global buffer in bytes (maximum of 1024 bytes)</param>
        /// <param name="localSize">The size of the local buffer in bytes (maximum of 64 bytes)</param>
        internal Command(CommandType commandType, ushort globalSize, int localSize)
        {
            Initialize(commandType, globalSize, localSize);
        }

        internal CommandType CommandType { get; set; }

        internal Response Response { get; set; }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Start a new command of a specific type
        /// </summary>
        /// <param name="commandType">The type of the command to start</param>
        internal void Initialize(CommandType commandType)
        {
            Initialize(commandType, 0, 0);
        }

        /// <summary>
        /// Start a new command of a specific type with a global and/or local buffer on the EV3 brick
        /// </summary>
        /// <param name="commandType">The type of the command to start</param>
        /// <param name="globalSize">The size of the global buffer in bytes (maximum of 1024 bytes)</param>
        /// <param name="localSize">The size of the local buffer in bytes (maximum of 64 bytes)</param>
        internal void Initialize(CommandType commandType, ushort globalSize, int localSize)
        {
            if (globalSize > 1024)
            {
                throw new ArgumentException("Global buffer must be less than 1024 bytes", nameof(globalSize));
            }

            if (localSize > 64)
            {
                throw new ArgumentException("Local buffer must be less than 64 bytes", nameof(localSize));
            }

            _stream = new MemoryStream();
            _writer = new BinaryWriter(_stream);
            Response = ResponseManager.CreateResponse();

            CommandType = commandType;

            // 2 bytes (this gets filled in later when the user calls ToBytes())
            _writer.Write((ushort)0xffff);

            // 2 bytes
            _writer.Write(Response.Sequence);

            // 1 byte
            _writer.Write((byte)commandType);

            if (commandType == CommandType.DirectReply || commandType == CommandType.DirectNoReply)
            {
                // 2 bytes (llllllgg gggggggg)
                _writer.Write((byte)globalSize); // lower bits of globalSize
                _writer.Write((byte)((localSize << 2) | (globalSize >> 8) & 0x03)); // upper bits of globalSize + localSize
            }
        }

        internal void AddOpcode(Opcode opcode)
        {
            // 1 or 2 bytes (opcode + subcmd, if applicable)
            // I combined opcode + sub into ushort where applicable, so we need to pull them back apart here
            if (opcode > Opcode.Tst)
            {
                _writer.Write((byte)((ushort)opcode >> 8));
            }

            _writer.Write((byte)opcode);
        }

        internal void AddOpcode(SystemOpcode opcode)
        {
            _writer.Write((byte)opcode);
        }

        internal void AddGlobalIndex(byte index)
        {
            // 0xe1 = global index, long format, 1 byte
            _writer.Write((byte)0xe1);
            _writer.Write(index);
        }

        internal void AddParameter(byte parameter)
        {
            // 0x81 = long format, 1 byte
            _writer.Write((byte)ArgumentSize.Byte);
            _writer.Write(parameter);
        }

        internal void AddParameter(sbyte parameter)
        {
            // 0x81 = long format, 1 byte
            _writer.Write((byte)ArgumentSize.Byte);
            _writer.Write(parameter);
        }

        internal void AddParameter(short parameter)
        {
            // 0x82 = long format, 2 bytes
            _writer.Write((byte)ArgumentSize.Short);
            _writer.Write(parameter);
        }

        internal void AddParameter(ushort parameter)
        {
            // 0x82 = long format, 2 bytes
            _writer.Write((byte)ArgumentSize.Short);
            _writer.Write(parameter);
        }

        internal void AddParameter(uint parameter)
        {
            // 0x83 = long format, 4 bytes
            _writer.Write((byte)ArgumentSize.Int);
            _writer.Write(parameter);
        }

        internal void AddParameter(string s)
        {
            // 0x84 = long format, null terminated string
            _writer.Write((byte)ArgumentSize.String);
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            _writer.Write(bytes);
            _writer.Write((byte)0x00);
        }

        // Raw methods below don't get format specifier added prior to the data itself...these are used in system commands (only?)
        internal void AddRawParameter(byte parameter)
        {
            _writer.Write(parameter);
        }

        internal void AddRawParameter(ushort parameter)
        {
            _writer.Write(parameter);
        }

        internal void AddRawParameter(uint parameter)
        {
            _writer.Write(parameter);
        }

        internal void AddRawParameter(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            _writer.Write(bytes);
            _writer.Write((byte)0x00);
        }

        internal void AddRawParameter(byte[] data, int index, int count)
        {
            _writer.Write(data, index, count);
        }

        internal byte[] ToBytes()
        {
            byte[] buff = _stream.ToArray();

            // size of data, not including the 2 size bytes
            ushort size = (ushort)(buff.Length - 2);

            // little-endian
            buff[0] = (byte)size;
            buff[1] = (byte)(size >> 8);

            return buff;
        }

        internal void Close()
        {
            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }

            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }
    }
}
