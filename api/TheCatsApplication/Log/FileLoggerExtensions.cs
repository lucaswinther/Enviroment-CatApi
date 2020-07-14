using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TheCatsDomain.Interfaces.Repository;

namespace TheCatsApplication.Log
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddElasticProvider<TIntegration>(this ILoggingBuilder builder, TIntegration integration) where TIntegration : IElasticIntegration
        {
            builder.Services.AddSingleton<ILoggerProvider, ElasticLoggerProvider<TIntegration>>(p => new ElasticLoggerProvider<TIntegration>((_, logLevel) => logLevel >= LogLevel.Debug, integration));
            return builder;
        }
    }
}
