using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteExperienceApi.Data.Aggregates;

public interface IQuoteAggregateRepository
{
    Task<QuoteAggregate> GetById(string id);
    Task<IList<QuoteAggregate>> ToQuoteAggregates(IEnumerable<QuoteRequest> quotes);
}
