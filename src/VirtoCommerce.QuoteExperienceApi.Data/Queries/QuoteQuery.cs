using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.QuoteExperienceApi.Data.Aggregates;

namespace VirtoCommerce.QuoteExperienceApi.Data.Queries;

public class QuoteQuery : Query<QuoteAggregate>
{
    public string Id { get; set; }

    public override IEnumerable<QueryArgument> GetArguments()
    {
        yield return Argument<NonNullGraphType<StringGraphType>>(nameof(Id));
    }

    public override void Map(IResolveFieldContext context)
    {
        Id = context.GetArgument<string>(nameof(Id));
    }
}
