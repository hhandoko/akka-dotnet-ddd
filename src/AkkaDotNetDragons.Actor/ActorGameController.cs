// -----------------------------------------------------------------------
// <copyright file="GameController.cs">
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
    using System;

    using Akka.Actor;

    using AkkaDotNetDragons.Core;
    using AkkaDotNetDragons.Dto;

    /// <summary>
    /// The actor-backed game environment controller.
    /// </summary>
    public class ActorGameController : IGameController, IDisposable
    {
        /// <summary>
        /// The logger context.
        /// </summary>
        private const string CONTEXT = "game-ctrl";

        /// <summary>
        /// Akka actor system instance.
        /// </summary>
        private readonly ActorSystem ActorSystem;

        /// <summary>
        /// The service controller.
        /// </summary>
        private readonly IServiceController ServiceController;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger Logger;

        /// <summary>
        /// Creates a new instance of <see cref="ActorGameController"/>.
        /// </summary>
        /// <param name="name">The environment name.</param>
        /// <param name="serviceController">The service controller.</param>
        /// <param name="logger">The logger.</param>
        public ActorGameController(string name, IServiceController serviceController, ILogger logger)
        {
            Logger = logger;

            using (Logger.Log(CONTEXT, "Initialising actor system"))
            {
                ActorSystem = ActorSystem.Create(name);
                ServiceController = serviceController;
            }
        }

        /// <summary>
        /// The General actor reference (heroes' supervisor).
        /// </summary>
        private IActorRef Environment { get; set; }

        /// <summary>
        /// Initialise the game environment.
        /// </summary>
        public void Init()
        {
            using (Logger.Log(CONTEXT, "Initialising top-level actors"))
            {
                Environment = ActorSystem.ActorOf(Props.Create(() => new Environment(ServiceController, Logger)), "environment");
            }
        }
        
        /// <summary>
        /// Get the environment status information.
        /// </summary>
        /// <returns>The ternary tuple containing the environment status, list of registered players, and dragon opponents.</returns>
        public Tuple<EnvironmentStatus, string[], string[]> GetEnvironmentStatus()
        {
            return Environment.Ask<Tuple<EnvironmentStatus, string[], string[]>>("status").Result;
        }
        
        /// <summary>
        /// Start the players' registration.
        /// </summary>
        public void Start()
        {
            using (Logger.Log(CONTEXT, "Start players' registration"))
            {
                Environment.Tell("start");
            }
        }
        
        /// <summary>
        /// Reset the game environment.
        /// </summary>
        public void Reset()
        {
            using (Logger.Log(CONTEXT, "Resetting the game environment"))
            {
                Environment.Tell("reset");
            }
        }

        /// <summary>
        /// Register the hero for the battle.
        /// </summary>
        /// <param name="slug">The hero URL-friendly name.</param>
        /// <param name="hash">The hero avater hash.</param>
        /// <param name="name">The hero name.</param>
        /// <returns>True if the hero has been successfully registered.</returns>
        public bool Register(string slug, string hash, string name)
        {
            using (Logger.Log(CONTEXT, "Registering hero for the battle"))
            {
                return Environment.Ask<bool>(new HeroRegisterMessage(hash, slug, name)).Result;
            }
        }

        /// <summary>
        /// Deregister the hero for the battle.
        /// </summary>
        /// <param name="heroName">The hero name.</param>
        /// <returns>True if the hero has been successfully deregistered.</returns>
        public bool Deregister(string heroName)
        {
            using (Logger.Log(CONTEXT, "Deregistering hero for the battle"))
            {
                return Environment.Ask<bool>(new HeroDeregisterMessage(heroName)).Result;
            }
        }
        
        /// <summary>
        /// Attack the dragon.
        /// </summary>
        /// <param name="heroName">The hero name.</param>
        /// <param name="dragonName">The dragon name.</param>
        public void Attack(string heroName, string dragonName)
        {
            using (Logger.Log(CONTEXT, "Player attacking the dragon"))
            {
                Environment.Tell(new HeroAttackMessage(heroName, dragonName));
            }
        }
        
        /// <summary>
        /// Defend from attack.
        /// </summary>
        /// <param name="heroName">The hero name.</param>
        public void Defend(string heroName)
        {
            using (Logger.Log(CONTEXT, "Player defending from attack"))
            {
                Environment.Tell(new HeroDefendMessage(heroName));
            }
        }

        /// <summary>
        /// Dispose the game environment.
        /// </summary>
        public void Dispose()
        {
            using (Logger.Log(CONTEXT, "Shutting down actor system"))
            {
                ActorSystem.Shutdown();
            }
        }
    }
}
