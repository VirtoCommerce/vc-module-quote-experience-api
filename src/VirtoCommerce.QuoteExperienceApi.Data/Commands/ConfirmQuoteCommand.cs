using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.ExperienceApiModule.XOrder;

namespace VirtoCommerce.QuoteExperienceApi.Data.Commands;

public class ConfirmQuoteCommand : ICommand<CustomerOrderAggregate>
{
    public string Id { get; set; }
}

public class ConfirmQuoteCommandType : InputObjectGraphType<ConfirmQuoteCommand>
{
    public ConfirmQuoteCommandType()
    {
        Field(x => x.Id);
    }
}
