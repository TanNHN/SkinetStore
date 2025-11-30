using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middlewares;

public class ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(env, context, ex);
        }
    }

    private static Task HandleExceptionAsync(IHostEnvironment env, HttpContext context, Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        APIErrorResponse res = env.IsDevelopment() ?
        new(context.Response.StatusCode, ex.Message, ex.StackTrace) :
        new(context.Response.StatusCode, ex.Message, "Internal server error!");

        // How about 
        // APIErrorResponse res = env.IsDevelopment() ?
        // new(500, ex.Message, ex.StackTrace) :
        // new(500, ex.Message, "Internal server error!");

        JsonSerializerOptions options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

        string json = JsonSerializer.Serialize(res, options);
        return context.Response.WriteAsync(json);
    }
}
