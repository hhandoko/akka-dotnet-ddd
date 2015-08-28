// -----------------------------------------------------------------------
// <copyright file="Dragon.cs">
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

    /// <summary>
    /// The Dragon.
    /// </summary>
    public class Dragon : UntypedActor
    {
        /// <summary>
        /// The logger context.
        /// </summary>
        private const string CONTEXT = "drg-actor";

        /// <summary>
        /// The logger.
        /// </summary>
        private static ILogger Logger;

        /// <summary>
        /// The dragon's default health points.
        /// </summary>
        private static int DefaultHealthPoints = 10000;
        
        /// <summary>
        /// The pre-start event hook.
        /// </summary>
        protected override void PreStart()
        {
        }
        
        /// <summary>
        /// Creates a new instance of <see cref="Dragon"/>.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public Dragon(ILogger logger)
        {
            Logger = logger;

            HealthPoints = DefaultHealthPoints;
        }

        /// <summary>
        /// Gets or sets the current health points.
        /// </summary>
        private int HealthPoints { get; set; }

        /// <summary>
        /// Gets or sets the attack cancellable scheduler.
        /// </summary>
        private ICancelable AttackScheduler { get; set; }

        /// <summary>
        /// Gets the dragon's health status.
        /// </summary>
        private bool IsAlive => HealthPoints > 0;

        /// <summary>
        /// On message receive handler.
        /// </summary>
        /// <param name="message">The message.</param>
        protected override void OnReceive(object message)
        {
            if (message is string)
            {
                var msg = (string)message;
                switch (msg)
                {
                    case "alive":
                        using (Logger.Log(CONTEXT, $"Reporting `{Self.Path.Name}` status"))
                        {
                            Sender.Tell(IsAlive);
                        }
                        break;

                    case "attack":
                        using (Logger.Log(CONTEXT, $"`{Self.Path.Name}` was attacked by `{Sender.Path.Name}`"))
                        {
                            if (IsAlive)
                            {
                                HealthPoints = HealthPoints - 1000;

                                if (HealthPoints <= 0)
                                {
                                    HealthPoints = 0;
                                    Context.Parent.Tell("died");
                                }
                                else if (HealthPoints < DefaultHealthPoints * 0.15)
                                {
                                    Context.Parent.Tell("weak");
                                }
                            }
                        }
                        break;

                    case "died":
                        using (Logger.Log(CONTEXT, $"`{Self.Path.Name}` has died"))
                        {
                            AttackScheduler?.Cancel();
                            Context.Parent.Tell("died");
                        }
                        break;

                    default:
                        using (Logger.Log(CONTEXT, $"Resetting `{Self.Path.Name}` the dragon's state"))
                        {
                            HealthPoints = DefaultHealthPoints;
                            AttackScheduler =
                                Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(
                                    TimeSpan.FromSeconds(1),
                                    TimeSpan.FromSeconds(5),
                                    Context.Parent,
                                    new DragonAttackMessage(),
                                    Self);
                        }
                        break;
                }
            }
            else if (message is ResetMessage)
            {
                Self.Tell("begin");
            }
        }
    }
}
