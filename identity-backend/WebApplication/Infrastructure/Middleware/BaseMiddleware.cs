using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApplication.Infrastructure.Middleware
{
    public abstract class BaseMiddleware
    {
        public RequestDelegate Next { get; private set; }

        public BaseMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }

        public abstract Task Invoke(HttpContext context);
    }
}