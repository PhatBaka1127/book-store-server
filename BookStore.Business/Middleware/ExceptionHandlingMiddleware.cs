using BookStore.Business.Dto;
using BookStore.Data.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.Middleware
{
    public class ExceptionHandlingMiddleware : AuthorizeAttribute
    {
        public RequestDelegate requestDelegate;
        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }
        private static Task HandleException(HttpContext context, Exception ex)
        {
            var errorMessageObject = new ErrorResponse
            {
                result = false,
                message = ex.Message,
                code = "500"
            };
            var statusCode = (int)HttpStatusCode.InternalServerError;
            switch (ex)
            {
                case AuthorizedException:
                    errorMessageObject.code = "401";
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case ForbiddenException:
                    errorMessageObject.code = "403";
                    statusCode = (int)HttpStatusCode.Forbidden;
                    break;
                case NotFoundException:
                    errorMessageObject.code = "404";
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;
                case ConflictException:
                    errorMessageObject.code = "409";
                    statusCode = (int)HttpStatusCode.Conflict;
                    break;
                case FileException:
                    errorMessageObject.code = "400";
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
            }

            var errorMessage = JsonConvert.SerializeObject(errorMessageObject);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(errorMessage);
        }
    }
}
