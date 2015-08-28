// -----------------------------------------------------------------------
// <copyright file="IGameController.cs">
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
    using System;

    using AkkaDotNetDragons.Dto;

    /// <summary>
    /// The game controller interface.
    /// </summary>
    public interface IGameController
    {
        /// <summary>
        /// Initialise the game environment.
        /// </summary>
        void Init();

        /// <summary>
        /// Get the environment status information.
        /// </summary>
        /// <returns>The binary tuple containing the environment status and list of registered players.</returns>
        Tuple<EnvironmentStatus, string[], string[]> GetEnvironmentStatus();

        /// <summary>
        /// Start the players' registration.
        /// </summary>
        void Start();

        /// <summary>
        /// Reset the game environment.
        /// </summary>
        void Reset();

        /// <summary>
        /// Register the hero for the battle.
        /// </summary>
        /// <param name="slug">The hero URL-friendly name.</param>
        /// <param name="hash">The hero avater hash.</param>
        /// <param name="name">The hero name.</param>
        /// <returns>True if the hero has been successfully registered.</returns>
        bool Register(string slug, string hash, string name);

        /// <summary>
        /// Deregister the hero for the battle.
        /// </summary>
        /// <param name="heroName">The hero name.</param>
        /// <returns>True if the hero has been successfully registered.</returns>
        bool Deregister(string heroName);

        /// <summary>
        /// Attack the dragon.
        /// </summary>
        /// <param name="heroName">The hero name.</param>
        /// <param name="dragonName">The dragon name.</param>
        void Attack(string heroName, string dragonName);
        
        /// <summary>
        /// Defend from attack.
        /// </summary>
        /// <param name="heroName">The hero name.</param>
        void Defend(string heroName);
    }
}
