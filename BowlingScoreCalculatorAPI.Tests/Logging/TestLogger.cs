using Microsoft.Extensions.Logging;

namespace BowlingScoreCalculatorAPI.Tests.Logging
{
    public class TestLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly TestLoggerProvider _provider;

        public TestLogger(string categoryName, TestLoggerProvider provider)
        {
            _categoryName = categoryName;
            _provider = provider;
        }

        IDisposable ILogger.BeginScope<TState>(TState state) => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _provider.AddMessage(_categoryName, logLevel, formatter(state, exception));
        }

        private class NullScope : IDisposable
        {
            public static NullScope Instance { get; } = new NullScope();
            public void Dispose() { }
        }
    }
}
