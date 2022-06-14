﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using PredicaTask.Application.Exceptions;


namespace PredicaTask.API.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware 
    {
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong.");
            }
        }
    }
}