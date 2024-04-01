using Microsoft.AspNetCore.Authorization;

namespace Condo.Api;

public class CondominiumAuthorizationHandler : AuthorizationHandler<CondominiumMembershipRequirement, string>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CondominiumMembershipRequirement requirement,
        string resource)
    {
        var userCondoId = context.User.Claims.FirstOrDefault(c => c.Type == "cid")?.Value;
        if (userCondoId == resource)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}

public class CondominiumMembershipRequirement : IAuthorizationRequirement { }