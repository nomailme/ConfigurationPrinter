using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace ConfigurationPrinter.UnitTests
{
    public class XunitLoggerProvider : ILoggerProvider
    {
        private readonly Action<string> onWrite;
        private readonly ITestOutputHelper testOutputHelper;

        public XunitLoggerProvider(ITestOutputHelper testOutputHelper, Action<string> onWrite = null)
        {
            this.testOutputHelper = testOutputHelper;
            this.onWrite = onWrite;
        }

        public ILogger CreateLogger(string categoryName)
            => new XunitLogger(testOutputHelper, categoryName, onWrite);

        public void Dispose()
        {
        }
    }

    public class XunitLogger : ILogger
    {
        private readonly string categoryName;
        private readonly Action<string> onWrite;
        private readonly ITestOutputHelper testOutputHelper;

        public XunitLogger(ITestOutputHelper testOutputHelper, string categoryName, Action<string> onWrite)
        {
            this.testOutputHelper = testOutputHelper;
            this.categoryName = categoryName;
            this.onWrite = onWrite;
        }

        public IDisposable BeginScope<TState>(TState state)
            => NoopDisposable.Instance;

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            onWrite?.Invoke($"{categoryName} [{eventId}] {formatter(state, exception)}");

            testOutputHelper.WriteLine($"{categoryName} [{eventId}] {formatter(state, exception)}");
            if (exception != null)
            {
                testOutputHelper.WriteLine(exception.ToString());
            }
        }

        private class NoopDisposable : IDisposable
        {
            public static readonly NoopDisposable Instance = new NoopDisposable();

            public void Dispose()
            {
            }
        }
    }
}
