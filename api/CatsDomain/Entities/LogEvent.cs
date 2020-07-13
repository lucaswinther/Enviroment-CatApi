using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheCatsDomain.Entities
{
    public class LogEvent
    {
        /// Construtor sem parâmetros: Necessário para o framework ORM.
        /// Ele é protegido, para que não se permita instanciar o objeto as propriedades obrigatórias
        protected LogEvent()
        {
        }

        /// <summary>
        /// Construtor com as propriedades obrigatórias para o objeto existir
        /// </summary>
        /// <param name="eventDate"></param>
        /// <param name="eventType"></param>
        /// <param name="methodName"></param>
        /// <param name="executionTime"></param>
        /// <param name="executionTimeFrmt"></param>
        public LogEvent(DateTime eventDate, LogLevel eventType, string methodName, long executionTime)
        {
            EventDate = eventDate;
            EventTypeId = eventType;
            EventType = eventType.ToString();
            MethodName = methodName;
            ExecutionTime = executionTime;
        }

        public int LogEventId { get; private set; }
        public DateTime EventDate { get; private set; }
        public LogLevel EventTypeId { get; private set; }
        public string EventType { get; private set; }
        public string MethodName { get; private set; }
        public long ExecutionTime { get; private set; }
        public string ExecutionTimeFrmt { get; private set; }
        public string Description { get; private set; }

        // Métodos para atribuir informação a propriedade, validando a informação.

        public void SetLogEventId(int logEventId)
        {
            if (IdIsValid(logEventId))
                LogEventId = logEventId;
        }

        public void SetEventDate(DateTime eventDate)
        {
            if (EventDateIsValid(eventDate))
                EventDate = eventDate;
        }

        public void SetEventTypeId(LogLevel eventType)
        {
            if (EventTypeIdIsValid(eventType))
                EventTypeId = eventType;
        }

        public void SetEventType(LogLevel eventType)
        {
            if (EventTypeIsValid(eventType.ToString()))
                EventType = eventType.ToString();
        }

        public void SetMethodName(string methodName)
        {
            if (MethodNameIsValid(methodName))
                MethodName = methodName;
        }

        public void SetExecutionTime(long executionTime)
        {
            if (ExecutionTimeIsValid(executionTime))
                ExecutionTime = executionTime;
        }

        public void SetExecutionTimeFrmt(string executionTimeFrmt)
        {
            if (ExecutionTimeFrmtIsValid(executionTimeFrmt))
                ExecutionTimeFrmt = executionTimeFrmt;
        }

        public void SetDescription(string description)
        {
            if (DescriptionIsValid(description))
                Description = description;
        }

        /// <summary>
        /// Valida se o objeto está com as informações necessárias para ser persistido na base de dados
        /// </summary>
        /// <returns></returns>
        public bool IsValid() =>
            IdIsValid(LogEventId) &&
            EventDateIsValid(EventDate) &&
            EventTypeIdIsValid(EventTypeId) &&
            EventTypeIsValid(EventType) &&
            MethodNameIsValid(MethodName) &&
            ExecutionTimeIsValid(ExecutionTime) &&
            ExecutionTimeFrmtIsValid(ExecutionTimeFrmt) &&
            DescriptionIsValid(Description);

        // Métodos privados para consistir informações obrigatórias do objeto

        bool IdIsValid(int logEventId) => logEventId >= 0;
        bool EventDateIsValid(DateTime eventDate) => eventDate != null;
        bool EventTypeIdIsValid(LogLevel eventTypeId) => eventTypeId > 0;
        bool EventTypeIsValid(string eventType) => !string.IsNullOrEmpty(eventType) && eventType.Length <= 60;
        bool MethodNameIsValid(string methodName) => !string.IsNullOrEmpty(methodName) && methodName.Length <= 255;
        bool ExecutionTimeIsValid(long executionTime) => executionTime >= 0;
        bool ExecutionTimeFrmtIsValid(string executionTimeFrmt) => string.IsNullOrEmpty(executionTimeFrmt) ? true : executionTimeFrmt.Length <= 12;
        bool DescriptionIsValid(string description) => string.IsNullOrEmpty(description) ? true : description.Length <= 1024;
    }
}
