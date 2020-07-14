using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TheCatsDomain.Entities;
using TheCatsDomain.Interfaces.Repository;

namespace TheCatsApplication.Log
{
   public class ElasticLogger<TIntegration> : ILogger where TIntegration : IElasticIntegration
    {
        readonly Func<string, LogLevel, bool> _filter;
        readonly TIntegration _elkIntegration;
        readonly string _categoryName;
        readonly int maxLength = 8096;

        public ElasticLogger(Func<string, LogLevel, bool> filter, TIntegration elkIntegration, string categoryName)
        {
            _filter = filter;
            _elkIntegration = elkIntegration;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? null;

        public bool IsEnabled(LogLevel logLevel) => (_filter == null || _filter(_categoryName, logLevel));

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;
            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            var logBuilder = new StringBuilder();
            var message = formatter(state, exception);
            var timeStamp = 0;
            if (!string.IsNullOrEmpty(message))
            {
                var messageSplit = message.Split(";");
                if (messageSplit.Length >= 2)
                {
                    message = messageSplit[0];
                    int.TryParse(messageSplit[1], out timeStamp);
                }
                logBuilder.Append(message);
                logBuilder.Append(Environment.NewLine);
            }

            GetScope(logBuilder);

            if (exception != null)
                logBuilder.Append(exception.ToString());

            if (logBuilder.Capacity > maxLength)
                logBuilder.Capacity = maxLength;

            var logEvent = new LogEvent(DateTime.UtcNow, logLevel, _categoryName, timeStamp);
            logEvent.SetLogEventId(eventId.Id);
            logEvent.SetDescription(message);

            _elkIntegration.CreateIndex();
            _elkIntegration.AddDoc(logEvent);
        }

        IExternalScopeProvider ScopeProvider { get; set; }

        void GetScope(StringBuilder stringBuilder)
        {
            var scopeProvider = ScopeProvider;
            if (scopeProvider != null)
            {
                var initialLength = stringBuilder.Length;
                scopeProvider.ForEachScope((scope, state) =>
                {
                    var (builder, length) = state;
                    var first = length == builder.Length;
                    builder.Append(first ? "=> " : " => ").Append(scope);
                }, (stringBuilder, initialLength));
                stringBuilder.AppendLine();
            }
        }
    }
}
