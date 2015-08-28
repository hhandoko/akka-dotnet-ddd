// -----------------------------------------------------------------------
// <copyright file="Hero.cs">
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
    using Akka.Actor;

    using AkkaDotNetDragons.Core;

    /// <summary>
    /// The Hero.
    /// </summary>
    public class Hero : TypedActor, IHandle<HeroAttackMessage>, IHandle<string>
    {
        /// <summary>
        /// The logger context.
        /// </summary>
        private const string CONTEXT = "hro-actor";

        /// <summary>
        /// The logger.
        /// </summary>
        private static ILogger Logger;

        /// <summary>
        /// The dragon's default health points.
        /// </summary>
        private static int DefaultHealthPoints = 100;

        /// <summary>
        /// The pre-start event hook.
        /// </summary>
        protected override void PreStart()
        {
        }
        
        /// <summary>
        /// Creates a new instance of <see cref="Hero"/>.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public Hero(ILogger logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Gets or sets the current health points.
        /// </summary>
        private int HealthPoints { get; set; }

        /// <summary>
        /// Handle hero attack command.
        /// </summary>
        /// <param name="message"></param>
        public void Handle(HeroAttackMessage message)
        {
            using (Logger.Log(CONTEXT, $"`{Self.Path.Name}` is attacking `{message.Dragon}`"))
            {
                Context.ActorSelection($"/user/environment/{message.Dragon}").Tell("attack");
            }
        }

        /// <summary>
        /// Handle string messages.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(string message)
        {
            if (message == "begin")
            {
                using (Logger.Log(CONTEXT, $"`{Self.Path.Name}` is ready for battle"))
                {
                    HealthPoints = DefaultHealthPoints;
                }
            }
        }
    }
}
