using Microsoft.AspNetCore.Builder;

namespace Smart.Apartment.Api.Middleware
{
    public static class MiddlewareExtenstions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
