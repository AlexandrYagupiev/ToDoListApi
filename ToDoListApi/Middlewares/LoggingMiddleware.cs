using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http;

namespace ToDoListAPI.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Логирование входящего запроса
            LogIncomingRequest(context.Request);

            // Сохранение текущего ответа в переменную
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context); // Передаем управление следующему middleware

                // Логирование исходящего ответа
                LogOutgoingResponse(context.Response, responseBody);

                await responseBody.CopyToAsync(originalBodyStream); // Копируем ответ обратно в поток ответа
            }
        }

        private void LogIncomingRequest(HttpRequest request)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"HTTP {request.Method} {request.Path} {request.Protocol}");
            foreach (var header in request.Headers)
            {
                sb.AppendLine($"{header.Key}: {header.Value}");
            }

            Console.WriteLine(sb.ToString());
        }

        private void LogOutgoingResponse(HttpResponse response, Stream bodyStream)
        {
            bodyStream.Position = 0;
            var reader = new StreamReader(bodyStream);
            var body = reader.ReadToEnd();
            bodyStream.Position = 0;

            var sb = new StringBuilder();
            sb.AppendLine($"HTTP {response.StatusCode}");
            foreach (var header in response.Headers)
            {
                sb.AppendLine($"{header.Key}: {header.Value}");
            }
            sb.AppendLine();
            sb.AppendLine(body);

            Console.WriteLine(sb.ToString());
        }
    }
}