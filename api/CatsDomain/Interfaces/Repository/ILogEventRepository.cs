using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheCatsDomain.Entities;

namespace TheCatsDomain.Interfaces.Repository
{
    public interface ILogEventRepository
    {
        Task<ICollection<LogEvent>> GetLogEvents(DateTime startDate, DateTime finishDate, LogLevel eventType = LogLevel.None);
        Task AddLogEvent(LogEvent logEvent);
    }
}
