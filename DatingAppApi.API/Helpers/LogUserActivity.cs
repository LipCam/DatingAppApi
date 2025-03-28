using DatingAppApi.API.Extensions;
using DatingAppApi.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingAppApi.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (context.HttpContext.User.Identity?.IsAuthenticated != true)
                return;

            var userId = resultContext.HttpContext.User.GetUserId();

            var service = resultContext.HttpContext.RequestServices.GetRequiredService<IUsersService>();
            var user = await service.Find(userId);

            if (user == null)
                return;

            user.LastActive = DateTime.Now;
            await service.Update(user);
        }
    }
}
