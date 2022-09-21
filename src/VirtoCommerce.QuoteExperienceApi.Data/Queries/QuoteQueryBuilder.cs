using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.QuoteExperienceApi.Data.Aggregates;
using VirtoCommerce.QuoteExperienceApi.Data.Authorization;
using VirtoCommerce.QuoteExperienceApi.Data.Schemas;

namespace VirtoCommerce.QuoteExperienceApi.Data.Queries;

public class QuoteQueryBuilder : QueryBuilder<QuoteQuery, QuoteAggregate, QuoteType>
{
    protected override string Name => "quote";

    public QuoteQueryBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }

    protected override async Task AfterMediatorSend(IResolveFieldContext<object> context, QuoteQuery request, QuoteAggregate response)
    {
        await base.AfterMediatorSend(context, request, response);
        await Authorize(context, response, new QuoteAuthorizationRequirement());
    }
}
