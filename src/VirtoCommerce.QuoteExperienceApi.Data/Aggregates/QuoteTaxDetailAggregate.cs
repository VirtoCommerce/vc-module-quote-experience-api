using VirtoCommerce.CoreModule.Core.Tax;

namespace VirtoCommerce.QuoteExperienceApi.Data.Aggregates;

public class QuoteTaxDetailAggregate
{
    public TaxDetail Model { get; set; }
    public QuoteAggregate Quote { get; set; }
}
