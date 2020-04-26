using Microsoft.Extensions.Logging;

namespace AN.Integration.API.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogIsOk<TModel>(this ILogger logger, string code, string method) =>
            logger.LogInformation($"Received {typeof(TModel).Name} with code {code} for {method}");
    }
}
