using SimplePOS.Business.Exceptions;
using System.Net;
using System.Text.Json;

namespace SimplePOS.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message;

            switch(exception)
            {
                case NotFoundException notFound:
                    status = HttpStatusCode.NotFound;
                    message = notFound.Message;
                    break;
                default:
                    status = HttpStatusCode.InternalServerError;
                    message = "Ocurrió un error inesperado.";
                    logger.LogError(exception, "Error no manejado");
                    break;
            }

            var response = new
            {
                error = message,
                status = (int)status
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}
