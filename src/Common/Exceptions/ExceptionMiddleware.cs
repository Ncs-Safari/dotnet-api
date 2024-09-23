using System.Text.Json;
using Common.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Npgsql;

namespace Common.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStringLocalizer<ErrorsResource> _errorLocalizer;

        public ExceptionMiddleware(RequestDelegate next, IStringLocalizer<ErrorsResource> errorLocalizer)
        {
            _next = next;
            _errorLocalizer = errorLocalizer;
        }
        private static CustomServiceFault GetCustomException(Exception exception)
        {
            var customServiceFault = new CustomServiceFault
            {
                ErrorMessage = exception.Message,
                Source = exception.Source,
                StackTrace = exception.StackTrace,
                Target = exception.TargetSite.ToString(),

                // You should fill this property with details here.
                InnerExceptionMessage = exception.GetBaseException().ToString()
            };

            return customServiceFault;
        }
        public async Task Invoke(HttpContext context)
        {
            var customError = new ResponseDto<string> { IsSuccess = true };
            var response = context.Response;

            try
            {
                await _next(context);
            }
            catch (ApplicationException ex)
            {
                customError.IsSuccess = false;
                customError.Message = ex.Message;
            }
            catch (NpgsqlException ex)
            {
                customError.IsSuccess = false;
                customError.Message = _errorLocalizer["Error.Common.Database"];
                customError.MessageDetail = GetCustomException(ex);
                Logger.Instance.Error("SQL Exception:", ex);
            }
            catch (Exception ex)
            {
                customError.IsSuccess = false;
                customError.Message = _errorLocalizer["Error.Common.Application"];
                customError.MessageDetail = GetCustomException(ex);
                Logger.Instance.Error("Exception:", ex);
            }

            if (!customError.IsSuccess)
            {
                context.Response.ContentType = "application/json;";
                var result = JsonSerializer.Serialize(customError);
                await response.WriteAsync(result);
            }
        }
    }

}
