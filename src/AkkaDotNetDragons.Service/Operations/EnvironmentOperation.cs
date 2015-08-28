// -----------------------------------------------------------------------
// <copyright file="EnvironmentOperation.cs">
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
    using System.Runtime.Serialization;

    using AkkaDotNetDragons.Dto;

    using ServiceStack;

    /// <summary>
    /// Get existing environment information.
    /// </summary>
    [Api("Get existing environment information.")]
    [DataContract]
    [Route("/environment", "GET", Summary = @"Get existing environment information.")]
    public class GetEnvironment : IReturn<GetEnvironmentResponse>
    {
    }

    /// <summary>
    /// Register / unregister a player.
    /// </summary>
    [Api("Register / unregister a player.")]
    [ApiResponse(HttpStatusCode.Created, "Player registered successfully.")]
    [ApiResponse(HttpStatusCode.NotAcceptable, "Player with existing ID has already been registered.")]
    [DataContract]
    [Route("/environment/players/{Slug}", "POST, DELETE", Summary = @"Register / unregister a player.")]
    public class RegistrationCommand : IReturn
    {
        /// <summary>
        /// Gets or sets the user / player's URL-friendly name.
        /// </summary>
        [ApiMember(Name = "Slug",
            AllowMultiple = false,
            DataType = "string",
            IsRequired = true,
            Description = "The user / player's URL-friendly name.",
            ParameterType = "path")]
        [DataMember]
        public string Slug { get; set; }
        
        /// <summary>
        /// Gets or sets the user / player's avatar hash.
        /// </summary>
        [ApiMember(Name = "Hash",
            AllowMultiple = false,
            DataType = "string",
            IsRequired = true,
            Description = "The user / player's avatar hash.",
            ParameterType = "form")]
        [DataMember]
        public string Hash { get; set; }
        
        /// <summary>
        /// Gets or sets the user / player's name.
        /// </summary>
        [ApiMember(Name = "Name",
            AllowMultiple = false,
            DataType = "string",
            IsRequired = true,
            Description = "The user / player's name.",
            ParameterType = "form")]
        [DataMember]
        public string Name { get; set; }
    }
    
    /// <summary>
    /// Game environment message push.
    /// </summary>
    [Api("Game environment message push.")]
    [DataContract]
    [Route("/environment/messages", "POST", Summary = @"Game environment message push.")]
    public class EnvironmentMessage : IReturnVoid
    {
        /// <summary>
        /// Gets or sets the environment message type.
        /// </summary>
        [ApiMember(Name = "MessageType",
            AllowMultiple = false,
            DataType = "string",
            IsRequired = true,
            Description = "The environment message type.",
            ParameterType = "form")]
        [DataMember]
        public MessageTypes MessageType { get; set; }
        
        /// <summary>
        /// Gets or sets the sender name.
        /// </summary>
        [ApiMember(Name = "Name",
            AllowMultiple = false,
            DataType = "string",
            IsRequired = true,
            Description = "The sender name.",
            ParameterType = "form")]
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the environment message to push.
        /// </summary>
        [ApiMember(Name = "Message",
            AllowMultiple = false,
            DataType = "string",
            IsRequired = true,
            Description = "The environment message to push.",
            ParameterType = "form")]
        [DataMember]
        public string Message { get; set; }
    }

    /// <summary>
    /// Game environment state commands.
    /// </summary>
    [Api("Game environment state commands.")]
    [DataContract]
    [Route("/environment/{Command}", "POST", Summary = @"Game environment state commands.")]
    public class EnvironmentCommand : IReturnVoid
    {
        /// <summary>
        /// Gets or sets the environment state command.
        /// </summary>
        [ApiMember(Name = "Command",
            AllowMultiple = false,
            DataType = "string",
            IsRequired = true,
            Description = "The environment state command.",
            ParameterType = "path")]
        [ApiAllowableValues("Command", typeof(EnvironmentCommands))]
        [DataMember]
        public EnvironmentCommands Command { get; set; }
    }
}
