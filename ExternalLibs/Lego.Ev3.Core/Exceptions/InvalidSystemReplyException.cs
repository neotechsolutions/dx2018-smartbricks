// <copyright file="InvalidSystemReplyException.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception thrown when a reply from a system command is invalid
    /// </summary>
    [Serializable]
    public class InvalidSystemReplyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSystemReplyException"/> class.
        /// </summary>
        public InvalidSystemReplyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSystemReplyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidSystemReplyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSystemReplyException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InvalidSystemReplyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSystemReplyException"/> class.
        /// </summary>
        /// <param name="formatMessage">The format message.</param>
        /// <param name="status">The status.</param>
        internal InvalidSystemReplyException(string formatMessage, SystemReplyStatus status)
            : base(string.Format(formatMessage, status))
        {
            Status = status.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSystemReplyException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        protected InvalidSystemReplyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the reply status.
        /// </summary>
        public string Status { get; }
    }
}
