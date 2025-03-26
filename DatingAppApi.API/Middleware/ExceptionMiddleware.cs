using DatingAppApi.API.Errors;
using System.Net;
using System.Text.Json;

namespace DatingAppApi.API.Middleware
{
    public class ExceptionMiddleware
    {
        public RequestDelegate Next;
        public ILogger<ExceptionMiddleware> Logger;
        public IHostEnvironment Environment;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            Next = next;
            Logger = logger;
            Environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = Environment.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, (ex.InnerException != null ? ex.InnerException.Message : ex.StackTrace))
                    : new ApiException(context.Response.StatusCode, ex.Message, "Internal server error");

                var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, option);
                
                await context.Response.WriteAsync(json);
            }
        }
    }
}
