namespace JobBoardApp.Middleware
{
    /// <summary>
    /// Middleware that logs details about incoming HTTP requests and outgoing responses.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestLoggingMiddleware> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next <see cref="RequestDelegate"/> in the request pipeline.</param>
        /// <param name="logger">The <see cref="ILogger{RequestLoggingMiddleware}"/> used for logging.</param>
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        /// <summary>
        /// Invokes the middleware, logs request and response information, and measures processing time.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            logger.LogInformation("\n================================\nIncoming request: {method} {url} from {ipAddress} with: {userAgent}\n================================",
                context.Request.Method,
                context.Request.Path,
                context.Connection.RemoteIpAddress?.ToString(),
                context.Request.Headers.UserAgent
                );

            await next(context);

            stopwatch.Stop();

            logger.LogInformation("\n================================\nResponse: {statusCode}. Processing time:{elapsedMilliseconds}ms\n================================",
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds
            );
        }
    }
}
