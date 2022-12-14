using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.QuoteExperienceApi.Data.Aggregates;

namespace VirtoCommerce.QuoteExperienceApi.Data.Queries;

public class QuoteQueryHandler : IQueryHandler<QuoteQuery, QuoteAggregate>
{
    private readonly IQuoteAggregateRepository _quoteAggregateRepository;

    public QuoteQueryHandler(IQuoteAggregateRepository quoteAggregateRepository)
    {
        _quoteAggregateRepository = quoteAggregateRepository;
    }

    public Task<QuoteAggregate> Handle(QuoteQuery request, CancellationToken cancellationToken)
    {
        return _quoteAggregateRepository.GetById(request.Id);
    }
}
