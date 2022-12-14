using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.ExperienceApiModule.Core;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.QuoteExperienceApi.Data.Aggregates;
using VirtoCommerce.QuoteExperienceApi.Data.Queries;

namespace VirtoCommerce.QuoteExperienceApi.Data.Authorization;

public class QuoteAuthorizationHandler : AuthorizationHandler<QuoteAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, QuoteAuthorizationRequirement requirement)
    {
        var result = context.User.IsInRole(PlatformConstants.Security.SystemRoles.Administrator);

        if (!result)
        {
            var currentUserId = GetUserId(context);

            switch (context.Resource)
            {
                case QuoteAggregate quote:
                    result = quote.Model.CustomerId == currentUserId;
                    break;
                case QuotesQuery query:
                    query.CustomerId = currentUserId;
                    result = query.CustomerId != null;
                    break;
            }
        }

        if (result)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }


    private static string GetUserId(AuthorizationHandlerContext context)
    {
        return
            context.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            context.User.FindFirstValue("name") ??
            AnonymousUser.UserName;
    }
}
