using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteExperienceApi.Data.Aggregates;

public class QuoteTotalsAggregate
{
    public QuoteRequestTotals Model { get; set; }
    public QuoteAggregate Quote { get; set; }
}
