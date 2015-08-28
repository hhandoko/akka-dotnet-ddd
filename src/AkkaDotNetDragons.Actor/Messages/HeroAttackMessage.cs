// -----------------------------------------------------------------------
// <copyright file="HeroAttackMessage.cs">
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
    /// The hero attack action message.
    /// </summary>
    public class HeroAttackMessage
    {
        /// <summary>
        /// The hero ID.
        /// </summary>
        public readonly string Hero;

        /// <summary>
        /// The dragon ID.
        /// </summary>
        public readonly string Dragon;
        
        /// <summary>
        /// Creates a new instance of <see cref="HeroAttackMessage"/>.
        /// </summary>
        /// <param name="heroName">The hero name.</param>
        /// <param name="dragonName">The dragon name.</param>
        public HeroAttackMessage(string heroName, string dragonName)
        {
            Hero = heroName;
            Dragon = dragonName;
        }
    }
}
