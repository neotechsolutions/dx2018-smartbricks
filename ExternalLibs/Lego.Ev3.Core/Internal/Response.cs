// <copyright file="Response.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core
{
    using System;
    using System.Threading;

    internal class Response : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        internal Response(ushort sequence)
        {
            Sequence = sequence;
            Event = new ManualResetEvent(false);
        }

        public ReplyType ReplyType { get; set; }

        public ushort Sequence { get; }

        public ManualResetEvent Event { get; }

        public byte[] Data { get; set; }

        public SystemOpcode SystemCommand { get; set; }

        public SystemReplyStatus SystemReplyStatus { get; set; }

        public void Dispose()
        {
            Event?.Dispose();
        }
    }
}