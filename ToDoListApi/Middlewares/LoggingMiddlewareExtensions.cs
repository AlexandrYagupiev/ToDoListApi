using Microsoft.AspNetCore.Builder;

namespace ToDoListAPI.Middlewares
{
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}