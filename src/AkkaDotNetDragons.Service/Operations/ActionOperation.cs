// -----------------------------------------------------------------------
// <copyright file="ActionCommand.cs">
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
    using System.Runtime.Serialization;

    using AkkaDotNetDragons.Dto;

    using ServiceStack;

    /// <summary>
    /// User action commands.
    /// </summary>
    [Api("User action commands.")]
    [DataContract]
    [Route("/action/{Command}", "POST", Summary = @"User action commands.")]
    public class ActionCommand : IReturnVoid
    {
        /// <summary>
        /// Gets or sets the user action command.
        /// </summary>
        [ApiMember(Name = "Command",
            AllowMultiple = false,
            DataType = "string",
            IsRequired = true,
            Description = "The user action command.",
            ParameterType = "path")]
        [ApiAllowableValues("Command", typeof(ActionCommands))]
        [DataMember]
        public ActionCommands Command { get; set; }
        
        [ApiMember(Name = "Target",
            AllowMultiple = false,
            DataType = "string",
            IsRequired = false,
            Description = "The action target.",
            ParameterType = "form")]
        [DataMember]
        public string Target { get; set; }
    }
}
