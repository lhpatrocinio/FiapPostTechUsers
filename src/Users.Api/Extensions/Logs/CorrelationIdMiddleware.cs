﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace Users.Api.Extensions.Logs
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CorrelationIdOptions _options;

        public CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next ?? throw new ArgumentNullException(nameof(next));

            _options = options.Value;
        }

        public Task Invoke(HttpContext context)
        {
            if (TryGetCorrelationIdHeaderValue(context, out var correlationId))
            {
                using (LogContext.PushProperty("CorrelationId", correlationId))
                {
                    if (_options.IncludeInResponse)
                    {
                        context.Response.OnStarting(() =>
                        {
                            context.Response.Headers.Append(_options.Header, correlationId);
                            return Task.CompletedTask;
                        });
                    }

                    return _next(context);
                }
            }

            return _next(context);
        }

        private bool TryGetCorrelationIdHeaderValue(HttpContext context, out StringValues correlationId)
        {
            if (context.Request.Headers.TryGetValue(_options.Header, out correlationId))
                return true;

            correlationId = _options.CorrelationIdBuilder?.Invoke();

            if (string.IsNullOrWhiteSpace(correlationId))
            {
                return false;
            }

            context.Request.Headers.Append(_options.Header, correlationId);

            return true;
        }
    }
}
