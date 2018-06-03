// <copyright file="ResponseManager.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    internal static class ResponseManager
    {
        internal static readonly ConcurrentDictionary<int, Response> Responses = new ConcurrentDictionary<int, Response>();

        private static ushort _nextSequence = 0x0001;

        internal static Response CreateResponse()
        {
            ushort sequence = GetSequenceNumber();

            Response r = new Response(sequence);
            Responses[sequence] = r;
            return r;
        }

        internal static Task WaitForResponseAsync(Response r)
        {
            return Task.Run(() =>
            {
                if (r.Event.WaitOne(1000))
                {
                    Response completedResponse;
                    Responses.TryRemove(r.Sequence, out completedResponse);

                    completedResponse.Dispose();
                }
                else
                {
                    r.ReplyType = ReplyType.DirectReplyError;
                }
            });
        }

        internal static void HandleResponse(byte[] report)
        {
            if (report == null || report.Length < 3)
            {
                return;
            }

            ushort sequence = (ushort)(report[0] | (report[1] << 8));
            int replyType = report[2];

            if (sequence > 0)
            {
                if (!Responses.TryGetValue(sequence, out Response r))
                {
                    return;
                }

                if (Enum.IsDefined(typeof(ReplyType), replyType))
                {
                    r.ReplyType = (ReplyType)replyType;
                }

                if (r.ReplyType == ReplyType.DirectReply || r.ReplyType == ReplyType.DirectReplyError)
                {
                    r.Data = new byte[report.Length - 3];
                    Array.Copy(report, 3, r.Data, 0, report.Length - 3);
                }
                else if (r.ReplyType == ReplyType.SystemReply || r.ReplyType == ReplyType.SystemReplyError)
                {
                    if (Enum.IsDefined(typeof(SystemOpcode), (int)report[3]))
                    {
                        r.SystemCommand = (SystemOpcode)report[3];
                    }

                    if (Enum.IsDefined(typeof(SystemReplyStatus), (int)report[4]))
                    {
                        r.SystemReplyStatus = (SystemReplyStatus)report[4];
                    }

                    r.Data = new byte[report.Length - 5];
                    Array.Copy(report, 5, r.Data, 0, report.Length - 5);
                }

                r.Event.Set();
            }
        }

        private static ushort GetSequenceNumber()
        {
            if (_nextSequence == ushort.MaxValue)
            {
                _nextSequence++;
            }

            return _nextSequence++;
        }
    }
}
