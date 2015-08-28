// -----------------------------------------------------------------------
// <copyright file="RestServiceController.cs">
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

namespace AkkaDotNetDragons.Service
{
    using AkkaDotNetDragons.Core;
    using AkkaDotNetDragons.Dto;

    using RestSharp;

    using ServiceStack;

    /// <summary>
    /// The HTTP REST based service controller.
    /// </summary>
    public class RestServiceController : IServiceController
    {
        /// <summary>
        /// Creates a new instance of <see cref="RestServiceController"/>.
        /// </summary>
        /// <param name="host">The web application host URL.</param>
        public RestServiceController(string host)
        {
            Host = host;
        }

        /// <summary>
        /// Gets or sets the web application host URL.
        /// </summary>
        private string Host { get; }

        /// <summary>
        /// Push the current environment status to clients.
        /// </summary>
        public void PushStatus()
        {
            var command = new EnvironmentCommand { Command = EnvironmentCommands.Status }.ToPostUrl();
            var client = new RestClient(Host);
            var request = new RestRequest(command, Method.POST);
            client.ExecuteAsync(request, response => {});
        }
        
        /// <summary>
        /// Push a message to clients.
        /// </summary>
        /// <param name="messageType">The message type.</param>
        /// <param name="name">The message sender name.</param>
        /// <param name="message">The message to push.</param>
        public void PushMessage(MessageTypes messageType, string name, string message)
        {
            var command = new EnvironmentMessage().ToPostUrl();
            var client = new RestClient(Host);
            var request = new RestRequest(command, Method.POST);
            request.AddParameter("messageType", messageType);
            request.AddParameter("name", name);
            request.AddParameter("message", message);
            client.ExecuteAsync(request, response => {});
        }
    }
}
