// -----------------------------------------------------------------------
// <copyright file="IServiceController.cs">
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

namespace AkkaDotNetDragons.Core
{
    using AkkaDotNetDragons.Dto;

    /// <summary>
    /// The service controller interface.
    /// </summary>
    public interface IServiceController
    {
        /// <summary>
        /// Push the current environment status to clients.
        /// </summary>
        void PushStatus();
        
        /// <summary>
        /// Push a message to clients.
        /// </summary>
        /// <param name="messageType">The message type.</param>
        /// <param name="name">The message sender name.</param>
        /// <param name="message">The message to push.</param>
        void PushMessage(MessageTypes messageType, string name, string message);
    }
}
