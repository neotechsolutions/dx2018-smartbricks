// <copyright file="CommunicationErrorException.cs" company="Hubert de Fleurian">
//     Copyright 2018 - Hubert de Fleurian - Licensed under the Apache License 2.0
//     Original work from BrianPeek (https://github.com/BrianPeek/legoev3)
//     See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace Lego.Ev3.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using Lego.Ev3.Core.Enums;

    /// <summary>
    /// Exception thrown when an error has occurred in the communication layer
    /// </summary>
    [Serializable]
    public class CommunicationErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationErrorException"/> class.
        /// </summary>
        public CommunicationErrorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationErrorException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CommunicationErrorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationErrorException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CommunicationErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationErrorException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        protected CommunicationErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets or sets the connection type.
        /// </summary>
        public ConnectionType ConnectionType { get; set; }
    }
}
