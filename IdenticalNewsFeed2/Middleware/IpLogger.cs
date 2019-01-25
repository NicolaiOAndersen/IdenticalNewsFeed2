using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdenticalNewsFeed2.Middleware
{
    public class IpLogger
    {
        private readonly RequestDelegate _next;

        public IpLogger(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvoiceAsync(HttpContext context)
        {
            //flesh out.

        }
    }
}
