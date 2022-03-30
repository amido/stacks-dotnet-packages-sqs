using Amido.Stacks.SQS.Events;
using Microsoft.Extensions.Logging;

namespace Amido.Stacks.SQS.Logging
{
    /// <summary>
    /// Contains log definitions for CosmosDB component
    /// LoggerMessage.Define() creates a unique template for each log type
    /// The log template reduces the number of allocations and write logs faster to destination
    /// </summary>
    public static class LogDefinition
    {
        /// Failures with exceptions should be logged to respective failures(i.e: getByIdFailed) and then to logException in order to show them as separate entries in the logs(trace + exception
        private static readonly Action<ILogger, string, Exception> logException =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId((int)EventCode.GeneralException, nameof(EventCode.GeneralException)),
                "AWS SQS Exception: {Message}"
            );

        // Publishing
        private static readonly Action<ILogger, string, Exception> publishEventRequested =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId((int)EventCode.PublishEventRequested, nameof(EventCode.PublishEventRequested)),
                "AWS SQS: PublishAsync requested for CorrelationId:{CorrelationId}"
            );

        private static readonly Action<ILogger, string, Exception> publishEventCompleted =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId((int)EventCode.PublishEventCompleted, nameof(EventCode.PublishEventCompleted)),
                "AWS SQS: PublishAsync completed for CorrelationId:{CorrelationId}"
            );

        private static readonly Action<ILogger, string, string, Exception> publishEventFailed =
            LoggerMessage.Define<string, string>(
                LogLevel.Information,
                new EventId((int)EventCode.PublishEventFailed, nameof(EventCode.PublishEventFailed)),
                "AWS SQS: PublishAsync failed for CorrelationId:{CorrelationId}. Reason:{Reason}"
            );

        // Consumer
        private static readonly Action<ILogger, Exception> processEventsRequested =
            LoggerMessage.Define(
                LogLevel.Information,
                new EventId((int)EventCode.ProcessEventsRequested, nameof(EventCode.ProcessEventsRequested)),
                "AWS SQS: ProcessAsync requested."
            );

        private static readonly Action<ILogger, string, Exception> processEventsCompleted =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId((int)EventCode.ProcessEventsCompleted, nameof(EventCode.ProcessEventsCompleted)),
                "AWS SQS: ProcessAsync completed for Event:{Event}"
            );

        private static readonly Action<ILogger, string, Exception> processEventsFailed =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId((int)EventCode.ProcessEventsFailed, nameof(EventCode.ProcessEventsFailed)),
                "AWS SQS: ProcessAsync failed. Reason:{Reason}"
            );

        //Exception

        /// <summary>
        /// When an exception is present in the failure, it will be logged as exception message instead of trace.
        /// Logging messages with an exception will make them an exception and the trace will lose an entry, making harder to debug issues
        /// </summary>
        private static void LogException(ILogger logger, Exception ex)
        {
            if (ex != null)
                logException(logger, ex.Message, ex);
        }

        // Publishing
        public static void PublishEventRequested(this ILogger logger, string correlationId)
        {
            publishEventRequested(logger, correlationId, null);
        }
        
        public static void PublishEventCompleted(this ILogger logger, string correlationId)
        {
            publishEventCompleted(logger, correlationId, null);
        }
        
        public static void PublishEventFailed(this ILogger logger, string correlationId, string reason, Exception ex)
        {
            publishEventFailed(logger, correlationId, reason, null);
            LogException(logger, ex);
        }

        // Consuming
        public static void ProcessEventsRequested(this ILogger logger)
        {
            processEventsRequested(logger, null);
        }
        
        public static void ProcessEventsCompleted(this ILogger logger, string processedEvent)
        {
            processEventsCompleted(logger, processedEvent, null);
        }
        
        public static void ProcessEventsFailed(this ILogger logger, string reason, Exception ex)
        {
            processEventsFailed(logger, reason, null);
            LogException(logger, ex);
        }
    }
}