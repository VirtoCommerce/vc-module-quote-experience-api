using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteExperienceApi.Data.Aggregates;

public class QuoteTierPriceAggregate
{
    public TierPrice Model { get; set; }
    public QuoteAggregate Quote { get; set; }
}
