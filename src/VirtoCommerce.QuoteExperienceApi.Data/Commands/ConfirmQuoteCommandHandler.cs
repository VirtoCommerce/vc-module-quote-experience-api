using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using MediatR;
using VirtoCommerce.ExperienceApiModule.Core.Helpers;
using VirtoCommerce.ExperienceApiModule.XOrder;
using VirtoCommerce.QuoteExperienceApi.Data.Services;
using VirtoCommerce.QuoteModule.Core.Services;

namespace VirtoCommerce.QuoteExperienceApi.Data.Commands;

public class ConfirmQuoteCommandHandler : IRequestHandler<ConfirmQuoteCommand, CustomerOrderAggregate>
{
    private readonly IQuoteRequestService _quoteRequestService;
    private readonly IQuoteTotalsCalculator _totalsCalculator;
    private readonly IQuoteConverter _quoteConverter;
    private readonly ICustomerOrderAggregateRepository _orderAggregateRepository;

    protected virtual string ValidQuoteStatus => "Proposal sent";

    public ConfirmQuoteCommandHandler(
        IQuoteRequestService quoteRequestService,
        IQuoteTotalsCalculator totalsCalculator,
        IQuoteConverter quoteConverter,
        ICustomerOrderAggregateRepository orderAggregateRepository)
    {
        _quoteRequestService = quoteRequestService;
        _totalsCalculator = totalsCalculator;
        _quoteConverter = quoteConverter;
        _orderAggregateRepository = orderAggregateRepository;
    }

    public async Task<CustomerOrderAggregate> Handle(ConfirmQuoteCommand request, CancellationToken cancellationToken)
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

        quote.Totals = await _totalsCalculator.CalculateTotalsAsync(quote);

        var cart = _quoteConverter.ConvertToCart(quote);
        var order = await _orderAggregateRepository.CreateOrderFromCart(cart);

        if (order != null)
        {
            quote.Status = "Ordered";
            await _quoteRequestService.SaveChangesAsync(new[] { quote });
        }

        return order;
    }
}
