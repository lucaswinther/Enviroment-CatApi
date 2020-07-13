using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCatsDomain.Entities;
using TheCatsDomain.Interfaces.Repository;
using TheCatsRepository.Context;

namespace TheCatsRepository.Repositories
{
    public class LogEventRepository : ILogEventRepository
    {
        // Comandos base para serem concatenados
        const string queryBase =
            @"SELECT LogEventId, EventDate, EventType, MethodName, ExecutionTime, ExecutionTimeFrmt, Description
              FROM LogEvent";

        readonly TheCatDBContext theCatContext;

        /// <summary>
        /// Construtor da classe: Espera um DBContext responsável por acessar a base e que implementa os
        /// comandos de banco de dados
        /// </summary>
        /// <param name="theCatContext"></param>
        public LogEventRepository(TheCatDBContext theCatContext)
        {
            this.theCatContext = theCatContext;
        }

        /// <summary>
        /// Método traz a informação da tabela LogEvent conforme parâmetros informados
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ICollection<LogEvent>> GetLogEvents(DateTime startDate, DateTime finishDate, LogLevel eventType = LogLevel.None)
        {
            // Garante que se passar um período dentro da mesma data, pega inicial como 0 e final 23:59
            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
            finishDate = new DateTime(finishDate.Year, finishDate.Month, finishDate.Day, 23, 59, 59);
            using (var conn = theCatContext.GetConnection)
            {
                var qryJoin = string.Concat(queryBase, " WHERE EventDate BETWEEN @startDate and @finishDate");
                if (eventType != LogLevel.None)
                    qryJoin = string.Concat(qryJoin, $" AND EventType = '{eventType}'");
                var result = await conn.QueryAsync<LogEvent>(
                    qryJoin,
                    new
                    {
                        startDate,
                        finishDate
                    }
                );
                return result.ToList();
            }
        }

        /// <summary>
        /// Método adiciona um registro na tabela LogEvent, caso o objeto logEvent passado seja válido
        /// </summary>
        /// <param name="logEvent"></param>
        /// <returns></returns>
        public async Task AddLogEvent(LogEvent logEvent)
        {
            if (!logEvent.IsValid())
                return;
            else
            {
                var sqlCommand =
                    @"INSERT INTO LogEvent 
                        (EventDate, EventTypeId, EventType, MethodName, ExecutionTime, ExecutionTimeFrmt, Description) 
                      VALUES
                        (@EventDate, @EventTypeId, @EventType, @MethodName, @ExecutionTime, @ExecutionTimeFrmt, @Description)";
                using (var conn = theCatContext.GetConnection)
                {
                    await conn.ExecuteAsync(sqlCommand, logEvent);
                }
            }
        }
    }
}
