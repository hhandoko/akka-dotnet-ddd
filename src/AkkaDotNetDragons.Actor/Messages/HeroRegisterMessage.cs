// -----------------------------------------------------------------------
// <copyright file="HeroRegisterMessage.cs">
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

namespace AkkaDotNetDragons.Actor
{
    /// <summary>
    /// The register a hero message.
    /// </summary>
    public class HeroRegisterMessage
    {
        /// <summary>
        /// The hero's avatar hash.
        /// </summary>
        public readonly string Hash;

        /// <summary>
        /// The hero's URL-friendly name.
        /// </summary>
        public readonly string Slug;

        /// <summary>
        /// The hero's name.
        /// </summary>
        public readonly string Name;
        
        /// <summary>
        /// Creates a new instance of <see cref="HeroRegisterMessage"/>.
        /// </summary>
        /// <param name="hash">The hero's avatar hash.</param>
        /// <param name="slug">The hero's URL-friendly name.</param>
        /// <param name="name">The hero's name.</param>
        public HeroRegisterMessage(string hash, string slug, string name)
        {
            Hash = hash;
            Slug = slug;
            Name = name;
        }
    }
}
