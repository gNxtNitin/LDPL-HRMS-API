using ErrorAndExceptionHandling.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
namespace ExceptionHandlingMiddlewareLib
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IErrorLoggingService _errorLoggingService;

        public ExceptionHandlingMiddleware(RequestDelegate next, IErrorLoggingService errorLoggingService)
        {
            _next = next;
            _errorLoggingService = errorLoggingService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var controller = context.GetRouteValue("controller")?.ToString();
            var action = context.GetRouteValue("action")?.ToString();


            //// Access route data via Endpoint, after routing has happened
            //var endpoint = context.GetEndpoint(); // Get the endpoint of the current route
            //var controller = endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>()?.ControllerName ?? "UnknownController";
            //var action = endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>()?.ActionName ?? "UnknownAction";



            var userId = context.User?.Identity?.Name;

            await _errorLoggingService.LogError(exception, controller, action, userId);
            // await _errorLoggingService.LogError(exception, "Global", "Unknown", userId);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error. Please try again later.",
                Details = exception.Message  // Optional, expose only in dev
            };

            var responseJson = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(responseJson);
        }
    }
}
