using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace UserManagement.Infrastructure.Services
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                var validators = context.RequestServices.GetServices<IValidator>();
                foreach (var validator in validators)
                {
                    var result = await validator.ValidateAsync(new ValidationContext<object>(context.Request.Body));
                    if (!result.IsValid)
                    {
                        var errors = result.Errors.Select(error => error.ErrorMessage);
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await context.Response.WriteAsync(JsonSerializer.Serialize(errors));
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}