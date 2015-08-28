// -----------------------------------------------------------------------
// <copyright file="DebugOutputLogger.cs">
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
    using System.Diagnostics;

    /// <summary>
    /// Debug output logger factory.
    /// </summary>
    public class DebugOutputLogger : ILogger
    {
        /// <summary>
        /// The logger line indent.
        /// </summary>
        private static int IndentLevel = 0;

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="context">The logger context.</param>
        /// <param name="message">The message to log.</param>
        public IDisposable Log(string context, string message)
        {
            return new DebugOutputLoggerWorker(context, message);
        }

        /// <summary>
        /// Write logs to output console (debug only).
        /// </summary>
        private class DebugOutputLoggerWorker : IDisposable
        {
            public DebugOutputLoggerWorker(string context, string message)
            {
                Debug.WriteLine($"{Indent()}[{context}]: {message}");
                IndentLevel++;
            }
            
            /// <summary>
            /// Dispose the logger.
            /// </summary>
            public void Dispose()
            {
                IndentLevel--;
            }

            /// <summary>
            /// Indent debug output.
            /// </summary>
            /// <returns>The indentation characters.</returns>
            private string Indent()
            {
                return 
                    IndentLevel <= 0
                        ? string.Empty
                        : "└".PadLeft(IndentLevel * 2);
            }
        }
    }
}
