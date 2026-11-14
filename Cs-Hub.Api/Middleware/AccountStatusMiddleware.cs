using Cs_Hub.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Cs_Hub.Api.Middleware
{
    public class AccountStatusMiddleware
    {
        private readonly RequestDelegate _next;

        public AccountStatusMiddleware(RequestDelegate newxt) 
        {
            _next = newxt;
        }

        public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
        {
            if(context.User.Identity?.IsAuthenticated != true)
            {
                await _next(context);
                return;
            }

            if(context.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() is not null)
            {
                await _next(context);
                return;
            }

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!Guid.TryParse(userId, out var id))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var user = await dbContext.Users.AsNoTracking().SingleOrDefaultAsync( u =>  u.Id == id);
            if(user is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            if(user.AccountStatus.ToString() != "Active")
            {
                context.Response.StatusCode= StatusCodes.Status403Forbidden;
                return;
            }
            await _next(context);
        }
    }
}
