// -----------------------------------------------------------------------
// <copyright file="Environment.cs">
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
    using System.Collections.Generic;
    using System.Linq;

    using Akka.Actor;

    using AkkaDotNetDragons.Core;
    using AkkaDotNetDragons.Dto;

    /// <summary>
    /// The environment monitoring.
    /// </summary>
    public class Environment : ReceiveActor
    {
        /// <summary>
        /// The logger context.
        /// </summary>
        private const string CONTEXT = "env-actor";

        /// <summary>
        /// The environment status.
        /// </summary>
        private static EnvironmentStatus Status;

        /// <summary>
        /// The service controller;
        /// </summary>
        private static IServiceController ServiceController;

        /// <summary>
        /// The logger.
        /// </summary>
        private static ILogger Logger;

        /// <summary>
        /// The dragon actor.
        /// </summary>
        private static IDictionary<string, IActorRef> DragonActors;

        /// <summary>
        /// The hero actors.
        /// </summary>
        private static IDictionary<string, IActorRef> HeroActors;

        /// <summary>
        /// The pre-start event hook.
        /// </summary>
        protected override void PreStart()
        {
            Status = EnvironmentStatus.Ready;
            DragonActors =
                new Dictionary<string, IActorRef>
                {
                    { "bahamut", Context.ActorOf(Props.Create(() => new Dragon(Logger)), "bahamut") },
                    { "enywas", Context.ActorOf(Props.Create(() => new Dragon(Logger)), "enywas") }
                };
            HeroActors = new Dictionary<string, IActorRef>();

            // Add event subscription for the dragons
            foreach (var dragon in DragonActors.Values)
            {
                Context.System.EventStream.Subscribe(dragon, typeof(ResetMessage));
            }
        }
        
        /// <summary>
        /// Creates a new instance of <see cref="Environment"/>.
        /// </summary>
        /// <param name="serviceController">The service controller.</param>
        /// <param name="logger">The logger.</param>
        public Environment(IServiceController serviceController, ILogger logger)
        {
            ServiceController = serviceController;
            Logger = logger;

            // Set initial state
            Ready();
        }

        /// <summary>
        /// Base actor operations.
        /// </summary>
        private void BaseOperations()
        {
            Receive<HeroRegisterMessage>(msg => { using (Logger.Log(CONTEXT, "WARN: Player registration outside of `Ready` state"))
            {
                Sender.Tell(false);
            }});

            Receive<string>(msg => msg == "status", msg => { using (Logger.Log(CONTEXT, "Reporting the environment status"))
            {
                Sender.Tell(Tuple.Create(Status, HeroActors.Keys.ToArray(), DragonActors.Keys.ToArray()));
            }});
            
            Receive<string>(msg => msg == "reset", msg => { using (Logger.Log(CONTEXT, "Switching to `Ready`"))
            {
                Status = EnvironmentStatus.Ready;
                Context.System.EventStream.Publish(new ResetMessage());
                Become(Ready);
            }});
        }

        /// <summary>
        /// Environment is initialised / reset and is ready.
        /// </summary>
        private void Ready()
        {
            Receive<HeroRegisterMessage>(msg => { using (Logger.Log(CONTEXT, "Registering player"))
            {
                if (HeroActors.ContainsKey(msg.Slug))
                {
                    Sender.Tell(false);
                }
                else
                {
                    HeroActors.Add(msg.Slug, Context.ActorOf(Props.Create(() => new Hero(Logger)), msg.Slug));
                    Sender.Tell(true);
                }
            }});

            Receive<HeroDeregisterMessage>(msg => { using (Logger.Log(CONTEXT, "Deregistering player"))
            {
                if (HeroActors.ContainsKey(msg.Hero))
                {
                    HeroActors[msg.Hero].Tell(PoisonPill.Instance);
                    HeroActors.Remove(msg.Hero);
                    Sender.Tell(true);
                }
                else
                {
                    Sender.Tell(false);
                }
            }});

            Receive<string>(msg => msg == "start", msg => { using (Logger.Log(CONTEXT, "Switching to `Registering`"))
            {
                Context.System.Scheduler.ScheduleTellOnce(
                    TimeSpan.FromSeconds(10), Self, "begin", Self);

                Status = EnvironmentStatus.Registering;
                Become(Registering);
            }});

            BaseOperations();
        }

        /// <summary>
        /// Currently registering players.
        /// </summary>
        private void Registering()
        {
            Receive<string>(msg => msg == "begin", msg => { using (Logger.Log(CONTEXT, "Switching to `Active`"))
            {
                Status = EnvironmentStatus.Active;
                
                foreach (var dragon in DragonActors)
                {
                    dragon.Value.Tell("begin");
                }

                ServiceController.PushStatus();
                Become(Active);
            }});

            BaseOperations();
        }

        /// <summary>
        /// Game is currently in progress.
        /// </summary>
        private void Active()
        {
            Receive<HeroAttackMessage>(msg => { using (Logger.Log(CONTEXT, "Player attacks the dragon"))
            {
                HeroActors[msg.Hero].Forward(msg);
            }});

            Receive<HeroDefendMessage>(msg => { using (Logger.Log(CONTEXT, "Player defending"))
            {
                HeroActors[msg.Hero].Forward(msg);
            }});

            Receive<DragonAttackMessage>(msg => { using (Logger.Log(CONTEXT, "Dragon attacks"))
            {
                var keys = HeroActors.Keys.ToList();
                var random = new Random();
                var randomHero = keys[random.Next(keys.Count)];
                var hero = HeroActors[randomHero];
                hero.Tell(msg);
                ServiceController.PushMessage(MessageTypes.Dragon, Sender.Path.Name, $"`{Sender.Path.Name}` the dragon attacks `@{hero.Path.Name}`");
            }});

            Receive<string>(msg => msg == "weak", msg => { using (Logger.Log(CONTEXT, $"`{Sender.Path.Name}` the dragon looks pretty weak right now"))
            {
                ServiceController.PushMessage(MessageTypes.System, Sender.Path.Name, $"`{Sender.Path.Name}` the dragon looks weak now...");
            }});

            Receive<string>(msg => msg == "died", msg => { using (Logger.Log(CONTEXT, $"`{Sender.Path.Name}` the dragon had died"))
            {
                var results = DragonActors.Select(dragonActor => dragonActor.Value.Ask<bool>("alive").Result).ToList();
                if (results.TrueForAll(alive => alive == false))
                {
                    ServiceController.PushMessage(MessageTypes.System, Sender.Path.Name, $"The dragons have been defeated!");
                    Self.Tell("finish");
                }
            }});

            Receive<string>(msg => msg == "finish", msg => { using (Logger.Log(CONTEXT, "Switching to `Finished`"))
            {
                Status = EnvironmentStatus.Finished;
                ServiceController.PushStatus();
                Become(Finished);
            }});

            BaseOperations();
        }

        /// <summary>
        /// Game has ended.
        /// </summary>
        private void Finished()
        {
            Receive<string>(msg => msg == "new", msg => { using (Logger.Log(CONTEXT, "Switching to `Ready`"))
            {
                Status = EnvironmentStatus.Ready;
                Become(Ready);
            }});

            BaseOperations();
        }
    }
}
