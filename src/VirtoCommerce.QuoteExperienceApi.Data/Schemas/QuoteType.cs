using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.Extensions;
using VirtoCommerce.ExperienceApiModule.Core.Helpers;
using VirtoCommerce.ExperienceApiModule.Core.Schemas;
using VirtoCommerce.ExperienceApiModule.Core.Services;
using VirtoCommerce.QuoteExperienceApi.Data.Aggregates;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteExperienceApi.Data.Schemas;

public class QuoteType : ExtendableGraphType<QuoteAggregate>
{
    public QuoteType(IDynamicPropertyResolverService dynamicPropertyResolverService)
    {
        Field(x => x.Model.CancelledDate, nullable: true);
        Field(x => x.Model.CancelReason, nullable: true);
        Field(x => x.Model.ChannelId, nullable: true);
        Field(x => x.Model.Comment, nullable: true);
        Field(x => x.Model.Coupon, nullable: true);
        Field(x => x.Model.CustomerId, nullable: true);
        Field(x => x.Model.CustomerName, nullable: true);
        Field(x => x.Model.CreatedBy, nullable: true);
        Field(x => x.Model.CreatedDate, nullable: false);
        Field(x => x.Model.EmployeeId, nullable: true);
        Field(x => x.Model.EmployeeName, nullable: true);
        Field(x => x.Model.EnableNotification, nullable: false);
        Field(x => x.Model.ExpirationDate, nullable: true);
        Field(x => x.Model.Id, nullable: false);
        Field(x => x.Model.InnerComment, nullable: true);
        Field(x => x.Model.IsAnonymous, nullable: false);
        Field(x => x.Model.IsCancelled, nullable: false);
        Field(x => x.Model.IsLocked, nullable: false);
        Field(x => x.Model.LanguageCode, nullable: true);
        Field(x => x.Model.ModifiedBy, nullable: true);
        Field(x => x.Model.ModifiedDate, nullable: true);
        Field(x => x.Model.Number, nullable: false);
        Field(x => x.Model.ObjectType, nullable: true);
        Field(x => x.Model.OrganizationId, nullable: true);
        Field(x => x.Model.OrganizationName, nullable: true);
        Field(x => x.Model.ReminderDate, nullable: true);
        Field(x => x.Model.Status, nullable: true);
        Field(x => x.Model.StoreId, nullable: false);
        Field(x => x.Model.Tag, nullable: true);

        Field<CurrencyType>(nameof(QuoteRequest.Currency), resolve: context => context.Source.Currency);
        Field<MoneyType>(nameof(QuoteRequest.ManualRelDiscountAmount), resolve: context => context.Source.Model.ManualRelDiscountAmount.ToMoney(context.Source.Currency));
        Field<MoneyType>(nameof(QuoteRequest.ManualShippingTotal), resolve: context => context.Source.Model.ManualShippingTotal.ToMoney(context.Source.Currency));
        Field<MoneyType>(nameof(QuoteRequest.ManualSubTotal), resolve: context => context.Source.Model.ManualSubTotal.ToMoney(context.Source.Currency));

        ExtendableField<QuoteTotalsType>(nameof(QuoteRequest.Totals), resolve: context => context.Source.Totals);
        ExtendableField<ListGraphType<QuoteItemType>>(nameof(QuoteRequest.Items), resolve: context => context.Source.Items);
        ExtendableField<ListGraphType<QuoteAddressType>>(nameof(QuoteRequest.Addresses), resolve: context => context.Source.Model.Addresses);
        ExtendableField<ListGraphType<QuoteAttachmentType>>(nameof(QuoteRequest.Attachments), resolve: context => context.Source.Model.Attachments);
        ExtendableField<QuoteShipmentMethodType>(nameof(QuoteRequest.ShipmentMethod), resolve: context => context.Source.ShipmentMethod);
        ExtendableField<ListGraphType<QuoteTaxDetailType>>(nameof(QuoteRequest.TaxDetails), resolve: context => context.Source.TaxDetails);

        ExtendableField<ListGraphType<DynamicPropertyValueType>>(
            nameof(QuoteRequest.DynamicProperties),
            "Quote dynamic property values",
            QueryArgumentPresets.GetArgumentForDynamicProperties(),
            context => dynamicPropertyResolverService.LoadDynamicPropertyValues(context.Source.Model, context.GetArgumentOrValue<string>("cultureName")));
    }
}
