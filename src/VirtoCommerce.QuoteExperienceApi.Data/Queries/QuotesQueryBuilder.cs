using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.QuoteExperienceApi.Data.Aggregates;
using VirtoCommerce.QuoteExperienceApi.Data.Authorization;
using VirtoCommerce.QuoteExperienceApi.Data.Schemas;

namespace VirtoCommerce.QuoteExperienceApi.Data.Queries;

public class QuotesQueryBuilder : SearchQueryBuilder<QuotesQuery, QuoteAggregateSearchResult, QuoteAggregate, QuoteType>
{
    protected override string Name => "quotes";

    public QuotesQueryBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }

    protected override async Task BeforeMediatorSend(IResolveFieldContext<object> context, QuotesQuery request)
    {
        await base.BeforeMediatorSend(context, request);
        await Authorize(context, request, new QuoteAuthorizationRequirement());
    }
}
