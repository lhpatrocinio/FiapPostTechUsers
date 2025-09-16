using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System;
using Users.Api.Extensions.Logs.Filter;
using Users.Api.Extensions.Logs.Middleware;

namespace Users.Api.Extensions.Logs.Extension
{
    [ExcludeFromCodeCoverage]
    public static class LogRequestExtensions
    {
        public static IApplicationBuilder UseSimpleLogRequest(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            return app.UseMiddleware<LogSimpleRequestMiddleware>();
        }

        public static MvcOptions AddLogRequestFilter(this MvcOptions op)
        {
            op.Filters.Add<LogRequestActionFilter>();

            return op;
        }
    }
}

