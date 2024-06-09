using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace BowlingScoreCalculatorAPI.Tests.Logging
{
    public class TestLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentQueue<(string Category, LogLevel LogLevel, string Message)> _messages = new ConcurrentQueue<(string Category, LogLevel LogLevel, string Message)>();

        public ILogger CreateLogger(string categoryName) => new TestLogger(categoryName, this);

        public void Dispose() { }

        public void AddMessage(string category, LogLevel logLevel, string message)
        {
            _messages.Enqueue((category, logLevel, message));
        }

        public bool Contains(LogLevel logLevel, string message)
        {
            return _messages.Any(m => m.LogLevel == logLevel && m.Message.Contains(message));
        }
    }
}
