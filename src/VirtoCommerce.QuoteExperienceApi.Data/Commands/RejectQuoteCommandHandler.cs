using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using VirtoCommerce.ExperienceApiModule.Core.Helpers;
using VirtoCommerce.QuoteExperienceApi.Data.Aggregates;
using VirtoCommerce.QuoteModule.Core.Services;

namespace VirtoCommerce.QuoteExperienceApi.Data.Commands;

public class RejectQuoteCommandHandler : IRequestHandler<RejectQuoteCommand, QuoteAggregate>
{
    private readonly IQuoteRequestService _quoteRequestService;
    private readonly IQuoteAggregateRepository _quoteAggregateRepository;

    protected virtual string ValidQuoteStatus => "Proposal sent";

    public RejectQuoteCommandHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteAggregateRepository quoteAggregateRepository)
    {
        _quoteRequestService = quoteRequestService;
        _quoteAggregateRepository = quoteAggregateRepository;
    }

    public async Task<QuoteAggregate> Handle(RejectQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = (await _quoteRequestService.GetByIdsAsync(request.Id)).FirstOrDefault();

        if (quote == null)
        {
            return null;
        }

        if (quote.Status != ValidQuoteStatus)
        {
            throw new ExecutionError($"Quote status is not '{ValidQuoteStatus}'") { Code = Constants.ValidationErrorCode };
        }

        quote.Status = "Rejected";
        await _quoteRequestService.SaveChangesAsync(new[] { quote });

        return await _quoteAggregateRepository.GetById(quote.Id);
    }
}
