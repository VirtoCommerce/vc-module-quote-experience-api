using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.Extensions;
using VirtoCommerce.ExperienceApiModule.Core.Schemas;
using VirtoCommerce.QuoteExperienceApi.Data.Aggregates;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteExperienceApi.Data.Schemas;

public class QuoteTierPriceType : ObjectGraphType<QuoteTierPriceAggregate>
{
    public QuoteTierPriceType()
    {
        Field(x => x.Model.Quantity, nullable: false);
        Field<MoneyType>(nameof(TierPrice.Price), resolve: context => context.Source.Model.Price.ToMoney(context.Source.Quote.Currency));
    }
}
