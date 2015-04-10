﻿// Copyright © 2015 - Present RealDimensions Software, LLC
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// 
// You may obtain a copy of the License at
// 
// 	http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace chocolatey.package.verifier.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Infrastructure.Logging;
    using Moq;

    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }

    public class MockLogger : Mock<ILog>, ILog, ILog<MockLogger>
    {
        private readonly Lazy<ConcurrentDictionary<string, IList<string>>> _messages = new Lazy<ConcurrentDictionary<string, IList<string>>>();

        public ConcurrentDictionary<string, IList<string>> Messages
        {
            get { return _messages.Value; }
        }

        public IList<string> MessagesFor(LogLevel logLevel)
        {
            return _messages.Value.GetOrAdd(logLevel.ToString(), new List<string>());
        }

        public void InitializeFor(string loggerName)
        {
        }

        public void LogMessage(LogLevel logLevel, string message)
        {
            var list = _messages.Value.GetOrAdd(logLevel.ToString(), new List<string>());
            list.Add(message);
        }

        public void Debug(string message, params object[] formatting)
        {
            LogMessage(LogLevel.Debug, message.FormatWith(formatting));
            Object.Debug(message, formatting);
        }

        public void Debug(Func<string> message)
        {
            LogMessage(LogLevel.Debug, message());
            Object.Debug(message);
        }

        public void Info(string message, params object[] formatting)
        {
            LogMessage(LogLevel.Info, message.FormatWith(formatting));
            Object.Info(message, formatting);
        }

        public void Info(Func<string> message)
        {
            LogMessage(LogLevel.Info, message());
            Object.Info(message);
        }

        public void Warn(string message, params object[] formatting)
        {
            LogMessage(LogLevel.Warn, message.FormatWith(formatting));
            Object.Warn(message, formatting);
        }

        public void Warn(Func<string> message)
        {
            LogMessage(LogLevel.Warn, message());
            Object.Warn(message);
        }

        public void Error(string message, params object[] formatting)
        {
            LogMessage(LogLevel.Error, message.FormatWith(formatting));
            Object.Error(message, formatting);
        }

        public void Error(Func<string> message)
        {
            LogMessage(LogLevel.Error, message());
            Object.Error(message);
        }

        public void Fatal(string message, params object[] formatting)
        {
            LogMessage(LogLevel.Fatal, message.FormatWith(formatting));
            Object.Fatal(message, formatting);
        }

        public void Fatal(Func<string> message)
        {
            LogMessage(LogLevel.Fatal, message());
            Object.Fatal(message);
        }
    }
}