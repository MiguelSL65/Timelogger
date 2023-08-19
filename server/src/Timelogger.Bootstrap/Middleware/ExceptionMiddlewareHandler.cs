using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Timelogger.Api.Responses;
using Timelogger.Domain.Exceptions;

namespace Timelogger.Bootstrap.Middleware;

public class ExceptionMiddlewareHandler
{
    private readonly RequestDelegate _next;

    public ExceptionMiddlewareHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = GetErrorResponse(exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) response.StatusCode;

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response.ApplicationError));
    }

    private static (HttpStatusCode StatusCode, ApplicationError ApplicationError) GetErrorResponse(
        Exception exception)
    {
        return exception switch
        {
            TimeloggerBusinessException e => (HttpStatusCode.BadRequest,
                ApplicationError.ValidationException(e.Errors)),
            EntityNotFoundException e => (HttpStatusCode.NotFound,
                ApplicationError.EntityNotFound(e.Message)),
            _ => (HttpStatusCode.InternalServerError, ApplicationError.InternalServerError(exception.Message))
        };
    }
}