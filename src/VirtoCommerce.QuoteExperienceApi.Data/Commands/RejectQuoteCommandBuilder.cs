using System.Threading.Tasks;
using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.QuoteExperienceApi.Data.Aggregates;
using VirtoCommerce.QuoteExperienceApi.Data.Schemas;

namespace VirtoCommerce.QuoteExperienceApi.Data.Commands;

public class RejectQuoteCommandBuilder : CommandBuilder<RejectQuoteCommand, QuoteAggregate, RejectQuoteCommandType, QuoteType>
{
    protected override string Name => "rejectQuote";

    public RejectQuoteCommandBuilder(IMediator mediator, IAuthorizationService authorizationService)
        : base(mediator, authorizationService)
    {
    }


    protected override Task BeforeMediatorSend(IResolveFieldContext<object> context, RejectQuoteCommand request)
    {
        // TODO: Check permissions

        return base.BeforeMediatorSend(context, request);
    }
}
