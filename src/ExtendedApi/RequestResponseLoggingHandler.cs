using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Context;

namespace ExtendedApi
{
    public class RequestResponseLoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            // Set a correlation ID that will be used for any other logs on this request.
            var correlationId = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await LogRequest(request);
                var timer = Stopwatch.StartNew();

                // Execute Next Handler in the chain
                response = await base.SendAsync(request, cancellationToken);

                timer.Stop();
                response.Headers.Add("correlationId", correlationId);
                await LogResponse(request, response, timer.Elapsed);
            }

            return response;
        }

        private async Task LogRequest(HttpRequestMessage request)
        {
            if (request.Method != HttpMethod.Options) // don't log OPTION requests
            {
                if (request.Content == null) return;
                var requestMessage = await request.Content.ReadAsByteArrayAsync();

                var result = Encoding.UTF8.GetString(requestMessage);
                await Task.Run(() =>
                {
                    using (LogContext.PushProperty("Referrer", GetReferrerForLogging(request)))
                    using (LogContext.PushProperty("Headers", request.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value))))
                    using (LogContext.PushProperty("RequestBody", Encoding.UTF8.GetString(requestMessage), true))
                    {
                        Log.Debug("{Type} [{Method}] {Uri}", "REQUEST", request.Method, request.RequestUri);
                    }
                });
            }
        }

        private async Task LogResponse(HttpRequestMessage request, HttpResponseMessage response, TimeSpan processingTime)
        {
            if (response != null && request.Method != HttpMethod.Options)
            {
                var responseMessage = response.Content != null
                    ? await response.Content.ReadAsByteArrayAsync()
                    : Encoding.UTF8.GetBytes(string.Empty);

                await Task.Run(() =>
                {
                    using (LogContext.PushProperty("Referrer", GetReferrerForLogging(request)))
                    using (LogContext.PushProperty("Headers", response.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value))))
                    using (LogContext.PushProperty("ResponseBody", Encoding.UTF8.GetString(responseMessage), true))
                    {
                        Log.Debug("{Type} [{Method}] {Uri} -> {Time:0}ms {HttpStatus} {Reason}",
                            "RESPONSE",
                            request.Method,
                            request.RequestUri,
                            processingTime.TotalMilliseconds,
                            (int) response.StatusCode,
                            response.ReasonPhrase);
                    }
                });
            }
        }

        private string GetReferrerForLogging(HttpRequestMessage request)
        {
            if (request != null && request.Headers != null && request.Headers.Referrer != null)
            {
                return request.Headers.Referrer.ToString();
            }
            return null;
        }
    }
}