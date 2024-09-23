using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net;

namespace UserManagement.Application.Configuration
{
    public class IpWhitelistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _allowedIpAddresses;

        public IpWhitelistMiddleware(RequestDelegate next, IOptions<IpWhitelistOptions> options)
        {
            _next = next;
            _allowedIpAddresses = options.Value.AllowedIpAddresses;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // var remoteIpAddress = GetRemoteIpAddress(context); //fungsi digunakan untuk akses khusus server side only 
            var remoteIpAddress = context.Connection.RemoteIpAddress;

            if (remoteIpAddress != null && !IsIpAddressAllowed(remoteIpAddress))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            await _next(context);
        }

        private IPAddress GetRemoteIpAddress(HttpContext context) //fungsi digunakan untuk akses khusus server side only 
        {
            var request = context.Request;
            var remoteIpAddress = IPAddress.Parse("127.0.0.1");

            if (request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor) &&
                IPAddress.TryParse(forwardedFor.ToString().Split(',')[0].Trim(), out var forwardedIpAddress))
            {
                remoteIpAddress = forwardedIpAddress;
            }
            else if (request.HttpContext.Connection.RemoteIpAddress != null)
            {
                remoteIpAddress = request.HttpContext.Connection.RemoteIpAddress;
            }

            return remoteIpAddress;
        }

        private bool IsIpAddressAllowed(IPAddress ipAddress)
        {
            return _allowedIpAddresses.Any(allowedIpAddress =>
                IPAddress.TryParse(allowedIpAddress, out var parsedIpAddress) && parsedIpAddress.Equals(ipAddress));
        }
    }
    public class IpWhitelistOptions
    {
        public string[] AllowedIpAddresses { get; set; }
    }
}
