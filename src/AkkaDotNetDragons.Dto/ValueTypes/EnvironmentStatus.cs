// -----------------------------------------------------------------------
// <copyright file="EnvironmentStatus.cs">
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
    /// <summary>
    /// Environment status.
    /// </summary>
    public enum EnvironmentStatus
    {
        /// <summary>
        /// Environment is ready to start a new game.
        /// </summary>
        Ready = 10,

        /// <summary>
        /// Currently registering additional players.
        /// </summary>
        Registering = 20,

        /// <summary>
        /// Game is currently in progress.
        /// </summary>
        Active = 30,

        /// <summary>
        /// (Last) game has been completed.
        /// </summary>
        Finished = 40
    }
}
