using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.QuoteExperienceApi.Data.Aggregates;
using VirtoCommerce.QuoteModule.Core.Models;
using VirtoCommerce.QuoteModule.Core.Services;

namespace VirtoCommerce.QuoteExperienceApi.Data.Queries;

public class QuotesQueryHandler : IQueryHandler<QuotesQuery, QuoteAggregateSearchResult>
{
    private readonly IQuoteRequestService _quoteRequestService;
    private readonly IQuoteAggregateRepository _quoteAggregateRepository;

    public QuotesQueryHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository)
    {
        _quoteRequestService = quoteRequestService;
        _quoteAggregateRepository = quoteAggregateRepository;
    }

    public virtual async Task<QuoteAggregateSearchResult> Handle(QuotesQuery request, CancellationToken cancellationToken)
    {
        var criteria = GetSearchCriteria(request);
        var searchResult = await _quoteRequestService.SearchAsync(criteria);

        var result = AbstractTypeFactory<QuoteAggregateSearchResult>.TryCreateInstance();
        result.TotalCount = searchResult.TotalCount;
        result.Results = await _quoteAggregateRepository.ToQuoteAggregates(searchResult.Results);

        return result;
    }


    protected virtual QuoteRequestSearchCriteria GetSearchCriteria(QuotesQuery request)
    {
        var criteria = request.GetSearchCriteria<QuoteRequestSearchCriteria>();
        criteria.CustomerId = request.CustomerId;

        return criteria;
    }
}
