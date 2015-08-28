// -----------------------------------------------------------------------
// <copyright file="EnvironmentResponses.cs">
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

namespace AkkaDotNetDragons.Dto
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Get the existing environment information.
    /// </summary>
    [DataContract(Name = "Environment")]
    public class GetEnvironmentResponse
    {
        /// <summary>
        /// Gets or sets the environment status.
        /// </summary>
        [DataMember]
        public EnvironmentStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the currently registered players.
        /// </summary>
        [DataMember]
        public string[] Players { get; set; }
        
        /// <summary>
        /// Gets or sets the current dragon opponents.
        /// </summary>
        [DataMember]
        public string[] Dragons { get; set; }
    }
}
