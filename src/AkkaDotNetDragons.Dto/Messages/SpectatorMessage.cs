// -----------------------------------------------------------------------
// <copyright file="SpectatorMessage.cs">
//   Copyright (c) 2015 Akka.NET Dragons Demo contributors
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace AkkaDotNetDragons.Dto.Messages
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Message for spectators.
    /// </summary>
    [DataContract]
    public class SpectatorMessage
    {
        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        [DataMember]
        public MessageTypes MessageType { get; set; }

        /// <summary>
        /// Gets or sets the avatar URL.
        /// </summary>
        [DataMember]
        public string Avatar { get; set; }

        /// <summary>
        /// Gets or sets the sender name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }
}
