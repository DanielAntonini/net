using System;
using System.Net;
using System.Threading.Tasks;
using Example.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Examples.Api.Middlewares;

public class ExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var customException = e as BusinessException;
            if (customException != null)
            {
                // get the response code and message
                response.StatusCode = (int)customException.StatusCode;
                await response.WriteAsync( JsonConvert.SerializeObject(new { Text = customException.Message, Code = customException.CustomCode}));    
            }
            else
            {
                // get the response code and message
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await response.WriteAsync( JsonConvert.SerializeObject(new { Text = e.Message}));    
            }

        }
    }
}