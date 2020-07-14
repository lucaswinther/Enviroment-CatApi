using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TheCatsDomain.Interfaces.Repository;

namespace TheCatsApplication.Log
{

public class ElasticLoggerProvider<TIntegration> : ILoggerProvider where TIntegration : IElasticIntegration
    {
        readonly Func<string, LogLevel, bool> _filter;
        readonly TIntegration _integration;

        public ElasticLoggerProvider(Func<string, LogLevel, bool> filter, TIntegration integration)
        {
            _filter = filter;
            _integration = integration;
        }

        public ILogger CreateLogger(string categoryName) => new ElasticLogger<TIntegration>(_filter, _integration, categoryName);

        public void Dispose()
        {
        }
    }
}
