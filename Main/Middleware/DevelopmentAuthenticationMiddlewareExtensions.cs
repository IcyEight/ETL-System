using Main.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Main.Extensions
{
    public static class DevelopmentAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseDevelopmentAuthentication(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DevelopmentAuthenticationMiddleware>();
        }
    }
}
