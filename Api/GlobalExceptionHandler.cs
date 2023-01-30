using Api.Exceptions;
using System.Net;
using System.Text.Json;

namespace Api
{
    public class GlobalExсeptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExсeptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidLineException error)
            {
                await ConstructResponse(context, HttpStatusCode.BadRequest, error.Message);
            }
            catch (CountLinesException error)
            {
                await ConstructResponse(context, HttpStatusCode.BadRequest, error.Message);
            }
            catch (FileNotFoundException error)
            {
                await ConstructResponse(context, HttpStatusCode.InternalServerError, error.Message);//NotFound???
            }
            catch (Exception error)
            {
                await ConstructResponse(context, HttpStatusCode.BadRequest, error.Message);
            }
        }

        private async Task ConstructResponse(HttpContext context, HttpStatusCode code, string message)
        {
            context.Response.ContentType = "applications/json";
            context.Response.StatusCode = (int)code;
            var result = JsonSerializer.Serialize(new { message = message });
            await context.Response.WriteAsync(result);
        }
    }
}
