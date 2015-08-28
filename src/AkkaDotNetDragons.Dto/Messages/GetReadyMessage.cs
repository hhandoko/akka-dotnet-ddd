// -----------------------------------------------------------------------
// <copyright file="DiceRollMessage.cs">
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
    /// A get ready for battle message.
    /// </summary>
    [DataContract]
    public class GetReadyMessage
    {
        /// <summary>
        /// Gets or sets the dragon names.
        /// </summary>
        [DataMember]
        public string[] Dragons { get; set; }
        
        /// <summary>
        /// Gets or sets the heroes' names.
        /// </summary>
        [DataMember]
        public Hero[] Heroes { get; set; }
    }

    /// <summary>
    /// The hero's details.
    /// </summary>
    [DataContract]
    public class Hero
    {
        /// <summary>
        /// Gets or sets the avatar hash.
        /// </summary>
        [DataMember]
        public string Hash { get; set; }
        
        /// <summary>
        /// Gets or sets the URL-friendly name.
        /// </summary>
        [DataMember]
        public string Slug { get; set; }
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}
