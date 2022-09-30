using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.ExperienceApiModule.XOrder;
using VirtoCommerce.ExperienceApiModule.XOrder.Schemas;

namespace VirtoCommerce.QuoteExperienceApi.Data.Commands;

public class ConfirmQuoteCommandBuilder : CommandBuilder<ConfirmQuoteCommand, CustomerOrderAggregate, ConfirmQuoteCommandType, CustomerOrderType>
{
    protected override string Name => "confirmQuote";

    public ConfirmQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }


    protected override Task BeforeMediatorSend(IResolveFieldContext<object> context, ConfirmQuoteCommand request)
    {
        // TODO: Check permissions

        return base.BeforeMediatorSend(context, request);
    }
}
