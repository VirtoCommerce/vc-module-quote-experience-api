using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.ExperienceApiModule.Core.Schemas;
using VirtoCommerce.QuoteExperienceApi.Data.Aggregates;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteExperienceApi.Data.Schemas;

public class QuoteShipmentMethodType : ExtendableGraphType<QuoteShipmentMethodAggregate>
{
    public QuoteShipmentMethodType()
    {
        Field(x => x.Model.ShipmentMethodCode, nullable: true);
        Field(x => x.Model.OptionName, nullable: true);
        Field(x => x.Model.LogoUrl, nullable: true);
        Field(x => x.Model.TypeName, nullable: true);
        Field<CurrencyType>(nameof(ShipmentMethod.Currency), resolve: context => context.Source.Quote.Currency);
        Field<MoneyType>(nameof(ShipmentMethod.Price), resolve: context => new Money(context.Source.Model.Price, context.Source.Quote.Currency));
    }
}
