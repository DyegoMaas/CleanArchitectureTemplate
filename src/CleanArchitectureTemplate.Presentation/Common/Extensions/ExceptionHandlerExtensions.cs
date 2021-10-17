using System;
using System.Net;
using CleanArchitectureTemplate.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureTemplate.Presentation.Common.Extensions
{
   public static class ExceptionHandlerExtension
   {
       public static void UseGlobalExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
       {
           app.UseExceptionHandler(builder =>
           {
               builder.Run(async httpContext =>
               {
                   var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
                   if (exceptionHandlerFeature != null)
                   {
                       var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");

                       if (exceptionHandlerFeature.Error is IBusinessException)
                           logger.LogWarning($"Unexpected error: {exceptionHandlerFeature.Error.Message}");
                       else
                           logger.LogError(exceptionHandlerFeature.Error, "Unexpected error");

                       var (httpStatusCode, message) = exceptionHandlerFeature.Error switch
                       {
                           ResourceNotFoundException exception => (HttpStatusCode.NotFound, exception.Message),
                           var error => (HttpStatusCode.InternalServerError, BuildErrorMessage(error))
                       };

                       httpContext.Response.ContentType = "application/json";
                       httpContext.Response.StatusCode = (int)httpStatusCode;

                       var json = new
                       {
                           Detailed = (string)null,
                           httpContext.Response.StatusCode,
                           Message = message,
                       };

                       await httpContext.Response.WriteAsJsonAsync(json);
                   }
               });
           });
       }

       private static string BuildErrorMessage(Exception exception)
       {
           return System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development
               ? exception.Message
               : "Unexpected erro while proccessing the request";
       }
   }
}
