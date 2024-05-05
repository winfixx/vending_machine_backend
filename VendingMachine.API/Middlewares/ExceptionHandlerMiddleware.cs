using System.Net;
using System.Text.Json;
using VendingMachine.API.Dto;
using VendingMachine.Core.Models;
using VendingMachine.Core.Models.Exceptions;

namespace VendingMachine.API.Middlewares
{
    public class ExceptionHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
            }
        }

        private static async Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;

            var exceptionResponse = new ExceptionResponse(
                statusCode,
                exception.Message ?? "Необработанная ошибка");

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string exceptionResponseJson = JsonSerializer.Serialize(
                exceptionResponse,
                jsonSerializerOptions);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(exceptionResponseJson);
            return;
        }
    }
}
