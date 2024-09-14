using System.Collections.Concurrent;

namespace JobBoardApp.Middleware
{
    public class LoginMiddleware
    {
        private readonly RequestDelegate next;
        private static readonly ConcurrentDictionary<string, int> LoginAttempts = new();
        public LoginMiddleware(RequestDelegate next)
        {
            this.next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/authentication/login"))
            {

                string ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                Console.WriteLine($"Login attempt for user:  from IP: {ip} at {DateTime.UtcNow}");

                LoginAttempts.AddOrUpdate(ip, 1, (key, value) => value + 1);


                if (LoginAttempts[ip] > 3)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.Response.WriteAsync("Too many login attempts. Please try again later.");
                    return;
                }
            }
            await next(context);
        }
        public static void ResetLoginAttempts(string ipAddress)
        {
            if (LoginAttempts.ContainsKey(ipAddress))
            {
                LoginAttempts[ipAddress] = 0;
            }
        }

    }

}
