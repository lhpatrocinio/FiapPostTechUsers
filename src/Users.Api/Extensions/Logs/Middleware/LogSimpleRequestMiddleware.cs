﻿using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using System.Linq;
using Serilog.Events;
using Serilog;


namespace Users.Api.Extensions.Logs.Middleware
{
    public class LogSimpleRequestMiddleware
    {
        const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        static readonly Serilog.ILogger Log = Serilog.Log.ForContext<LogSimpleRequestMiddleware>();


        readonly RequestDelegate _next;

        public LogSimpleRequestMiddleware(RequestDelegate next) => _next = next ?? throw new ArgumentNullException(nameof(next));

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            await HandleLogSimpleRequest(httpContext);
        }

        private async Task HandleLogSimpleRequest(HttpContext httpContext)
        {
            var start = Stopwatch.GetTimestamp();
            try
            {
                await _next(httpContext);
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

                var statusCode = httpContext.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                var log = level == LogEventLevel.Error ? LogForErrorContext(httpContext) : Log;
                log.Write(level, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, statusCode, elapsedMs);
            }
            catch (Exception ex)
            {
                LogForErrorContext(httpContext)
                    .Error(ex, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
                throw;
            }
        }

        static Serilog.ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = Log
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
                result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));

            return result;
        }

        static double GetElapsedMilliseconds(long start, long stop) => (stop - start) * 1000 / (double)Stopwatch.Frequency;
    }
}

