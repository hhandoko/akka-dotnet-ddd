// -----------------------------------------------------------------------
// <copyright file="AppHost.cs">
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

namespace AkkaDotNetDragons.Web
{
    using System.Collections.Generic;

    using AkkaDotNetDragons.Actor;
    using AkkaDotNetDragons.Core;
    using AkkaDotNetDragons.Service;

    using Funq;

    using ServiceStack;
    using ServiceStack.Api.Swagger;
    using ServiceStack.Configuration;
    using ServiceStack.Razor;
    using ServiceStack.Text;
    using ServiceStack.Validation;

    /// <summary>
    /// The Web Application host.
    /// </summary>
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// The application config parameters.
        /// </summary>
        public static AppConfig AppConfig;

        /// <summary>
        /// The application settings.
        /// </summary>
        public static AppSettings AppSettings;

        /// <summary>
        /// The actor-based game controller.
        /// </summary>
        public static ActorGameController ActorGameController;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppHost"/> class.
        /// </summary>
        public AppHost()
            : base("Akka.NET Dragons Demo", typeof(ServerEventsService).Assembly)
        {
        }

        /// <summary>
        /// Starts the application's services.
        /// </summary>
        public void Start()
        {
            ActorGameController.Init();
        }

        /// <summary>
        /// Shut down the application's services.
        /// </summary>
        public void ShutDown()
        {
            ActorGameController.Dispose();
        }

        /// <summary>
        /// Configure the Web Application host.
        /// </summary>
        /// <param name="container">The IoC container.</param>
        public override void Configure(Container container)
        {
            // Configure ServiceStack host
            ConfigureHost(container);

            // Configure JSON serialization properties
            ConfigureSerialization(container);

            // Configure application settings and configuration parameters
            ConfigureApplication(container);

            // Configure ServiceStack Fluent Validation plugin
            ConfigureValidation(container);

            // Configure ServiceStack Razor views
            ConfigureView(container);

            // Configure ServiceStack Server push technology
            ConfigureServerPush(container);

            // Configure various system tools / features
            ConfigureTools(container);
        }

        /// <summary>
        /// Configure ServiceStack host.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureHost(Container container)
        {
            SetConfig(new HostConfig
            {
                AppendUtf8CharsetOnContentTypes = new HashSet<string> { MimeTypes.Html },

                // Set to return JSON if no request content type is defined
                // e.g. text/html or application/json
                DefaultContentType = MimeTypes.Json,
#if DEBUG
                // Show StackTraces in service responses during development
                DebugMode = true,
#endif
                // Disable SOAP endpoints
                EnableFeatures = Feature.All.Remove(Feature.Soap)
            });
        }

        /// <summary>
        /// Configure JSON serialization properties.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureSerialization(Container container)
        {
            // Set JSON web services to return idiomatic JSON camelCase properties
            JsConfig.EmitCamelCaseNames = true;

            // Set JSON web services to return ISO8601 date format
            JsConfig.DateHandler = DateHandler.ISO8601;

            // Exclude type info during serialization,
            // except for UserSession DTO
            JsConfig.ExcludeTypeInfo = true;
        }

        /// <summary>
        /// Configure application settings and configuration.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureApplication(Container container)
        {
            // Set application settings
            AppSettings = new AppSettings();
            container.Register<IAppSettings>(AppSettings);

            // Set configuration parameters
            AppConfig = new AppConfig(AppSettings);
            // TODO: Inject AppConfig

            // Configure logger injection
            container.RegisterAutoWiredAs<DebugOutputLogger, ILogger>();

            // Set the service controller instance
            container.Register<IServiceController>(new RestServiceController("http://localhost:8888"));

            // Set the game controller instance
            ActorGameController = new ActorGameController("akka-dotnet-dragons", container.Resolve<IServiceController>(), container.Resolve<ILogger>());
            container.Register<IGameController>(ActorGameController);
        }

        /// <summary>
        /// Configure ServiceStack Fluent Validation plugin.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureValidation(Container container)
        {
            // Provide fluent validation functionality for web services
            Plugins.Add(new ValidationFeature());

            // TODO: Add validator's assembly for scanning
        }

        /// <summary>
        /// Configure ServiceStack Razor views.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureView(Container container)
        {
            // Enable ServiceStack Razor
            Plugins.Add(new RazorFormat());
        }

        /// <summary>
        /// Configure ServiceStack Server push technoloy.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureServerPush(Container container)
        {
            // Enable Server Sent Events
            Plugins.Add(new ServerEventsFeature());
        }

        /// <summary>
        /// Configure various system tools / features.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureTools(Container container)
        {
            // Add Postman and Swagger UI support
            Plugins.Add(new PostmanFeature());
            Plugins.Add(new SwaggerFeature());

            // Add CORS (Cross-Origin Resource Sharing) support
            Plugins.Add(new CorsFeature());
#if DEBUG
            // Development-time features
            // Add request logger
            // See: https://github.com/ServiceStack/ServiceStack/wiki/Request-logger
            Plugins.Add(new RequestLogsFeature());
#endif
        }
    }
}