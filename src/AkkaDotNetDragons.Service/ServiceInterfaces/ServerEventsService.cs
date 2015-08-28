// -----------------------------------------------------------------------
// <copyright file="ServerEventsService.cs">
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
    using System.Net;

    using AkkaDotNetDragons.Core;
    using AkkaDotNetDragons.Dto;
    using AkkaDotNetDragons.Dto.Messages;

    using ServiceStack;

    /// <summary>
    /// Server-sent Events web service.
    /// </summary>
    public class ServerEventsService : Service
    {
        /// <summary>
        /// Gets or sets the server events.
        /// </summary>
        public IServerEvents ServerEvents { get; set; }

        /// <summary>
        /// Gets or sets the game controller.
        /// </summary>
        public IGameController GameController { get; set; }

        /// <summary>
        /// Get the environment's status.
        /// </summary>
        /// <param name="request">The request.</param>
        public object Get(GetEnvironment request)
        {
            var status = GameController.GetEnvironmentStatus();

            return
                new GetEnvironmentResponse
                {
                    Status = status.Item1,
                    Players = status.Item2,
                    Dragons = status.Item3
                };
        }

        /// <summary>
        /// Register a player.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object Post(RegistrationCommand request)
        {
            var result = GameController.Register(request.Slug, request.Hash, request.Name);
            
            if (!result) throw new HttpError(HttpStatusCode.NotAcceptable, "DuplicatePlayerID");

            ServerEvents.NotifyChannel("spectator", new SpectatorMessage { MessageType = MessageTypes.Player, Name = request.Name, Avatar = request.Hash, Message = $"`@{request.Slug}` has joined the demo" });

            return new HttpResult(HttpStatusCode.Created);
        }

        /// <summary>
        /// Remove player's registration (deregister).
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object Delete(RegistrationCommand request)
        {
            var result = GameController.Deregister(request.Slug);

            if (result) ServerEvents.NotifyChannel("spectator", new SpectatorMessage { MessageType = MessageTypes.Player, Name = request.Name, Avatar = request.Hash, Message = $"`@{request.Slug}` has left the demo" });

            return new HttpResult(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Process environment commands.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Post(EnvironmentCommand request)
        {
            switch (request.Command)
            {
                case EnvironmentCommands.Start:
                    GameController.Start();
                    break;

                case EnvironmentCommands.Reset:
                    GameController.Reset();
                    break;
            }

            var status = GameController.GetEnvironmentStatus();
            switch (status.Item1)
            {
                case EnvironmentStatus.Registering:
                    ServerEvents.NotifyAll(new GetReadyMessage { Dragons = status.Item3 });
                    break;
                
                case EnvironmentStatus.Active:
                    ServerEvents.NotifyChannel("player", new EnvironmentStatusMessage { Status = status.Item1, Dragons = status.Item3 });
                    break;
            }

            ServerEvents.NotifyAll(new EnvironmentStatusMessage { Status = status.Item1, Dragons = status.Item3 });
        }

        /// <summary>
        /// Push environment messages.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Post(EnvironmentMessage request)
        {
            ServerEvents.NotifyChannel("spectator", new SpectatorMessage { MessageType = request.MessageType, Name = request.Name, Message = request.Message });
        }
        
        /// <summary>
        /// Process player's actions.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Post(ActionCommand request)
        {
            var user = Request.Cookies["hero"].Value;
            var name = Request.Cookies["hero_name"].Value;
            var hash = Request.Cookies["hero_hash"].Value;
            switch (request.Command)
            {
                case ActionCommands.Attack:
                    GameController.Attack(user, request.Target);
                    ServerEvents.NotifyChannel("spectator", new SpectatorMessage { MessageType = MessageTypes.Player, Name = name, Avatar = hash, Message = $"`@{user}` attacks `{request.Target}` the dragon" });
                    break;
            }
        }
    }
}
