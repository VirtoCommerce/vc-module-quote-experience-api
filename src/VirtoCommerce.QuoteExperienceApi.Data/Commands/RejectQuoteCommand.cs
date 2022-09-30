using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.QuoteExperienceApi.Data.Aggregates;

namespace VirtoCommerce.QuoteExperienceApi.Data.Commands;

public class RejectQuoteCommand : ICommand<QuoteAggregate>
{
    public string Id { get; set; }
}

public class RejectQuoteCommandType : InputObjectGraphType<RejectQuoteCommand>
{
    public RejectQuoteCommandType()
    {
        Field(x => x.Id);
    }
}
