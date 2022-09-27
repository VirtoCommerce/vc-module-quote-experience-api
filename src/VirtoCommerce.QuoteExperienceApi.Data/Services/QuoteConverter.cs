using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Tax;
using VirtoCommerce.ExperienceApiModule.Core.Extensions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.QuoteModule.Core.Models;
using CartAddress = VirtoCommerce.CartModule.Core.Model.Address;
using CartLineItem = VirtoCommerce.CartModule.Core.Model.LineItem;
using QuoteAddress = VirtoCommerce.QuoteModule.Core.Models.Address;
using QuoteShipmentMethod = VirtoCommerce.QuoteModule.Core.Models.ShipmentMethod;

namespace VirtoCommerce.QuoteExperienceApi.Data.Services;

public class QuoteConverter : IQuoteConverter
{
    protected virtual string InitialQuoteStatus => "Processing";

    public virtual QuoteRequest ConvertFromCart(ShoppingCart cart)
    {
        var result = AbstractTypeFactory<QuoteRequest>.TryCreateInstance();

        result.ChannelId = cart.ChannelId;
        result.Comment = cart.Comment;
        result.Coupon = cart.Coupon;
        result.Currency = cart.Currency;
        result.CustomerId = cart.CustomerId;
        result.CustomerName = cart.CustomerName;
        result.LanguageCode = cart.LanguageCode;
        result.OrganizationId = cart.OrganizationId;
        result.Status = InitialQuoteStatus;
        result.StoreId = cart.StoreId;

        result.Items = cart.Items?.Convert(FromCartItems);
        result.TaxDetails = cart.TaxDetails?.Convert(FromCartTaxDetails);
        result.DynamicProperties = cart.DynamicProperties?.Convert(FromCartDynamicProperties);

        result.Addresses = FromCartAddresses(cart);

        return result;
    }

    public ShoppingCart ConvertToCart(QuoteRequest quote)
    {
        var result = AbstractTypeFactory<ShoppingCart>.TryCreateInstance();

        result.Comment = quote.Comment;
        result.Coupon = quote.Coupon;
        result.Currency = quote.Currency;
        result.CustomerId = quote.CustomerId;
        result.CustomerName = quote.CustomerName;
        result.LanguageCode = quote.LanguageCode;
        result.OrganizationId = quote.OrganizationId;
        result.Status = InitialQuoteStatus;
        result.StoreId = quote.StoreId;

        result.Items = quote.Items?.Convert(ToCartItems);
        result.TaxDetails = quote.TaxDetails?.Convert(ToCartTaxDetails);
        result.DynamicProperties = quote.DynamicProperties?.Convert(ToCartDynamicProperties);

        result.Addresses = quote.Addresses?.Convert(ToCartAddresses);
        result.Shipments = quote.ShipmentMethod?.Convert(x => ToCartShipments(x, result.Addresses));
        result.Payments = ToCartPayments(quote, result.Addresses);

        return result;
    }


    protected virtual IList<QuoteItem> FromCartItems(ICollection<CartLineItem> items)
    {
        return items
            .Where(x => !x.IsRejected)
            .Select(FromCartItem)
            .ToList();
    }

    protected virtual QuoteItem FromCartItem(CartLineItem item)
    {
        var result = AbstractTypeFactory<QuoteItem>.TryCreateInstance();

        result.CatalogId = item.CatalogId;
        result.CategoryId = item.CategoryId;
        result.Comment = item.Note;
        result.Currency = item.Currency;
        result.ImageUrl = item.ImageUrl;
        result.ListPrice = item.ListPrice;
        result.Name = item.Name;
        result.ProductId = item.ProductId;
        result.SalePrice = item.SalePrice;
        result.Sku = item.Sku;
        result.TaxType = item.TaxType;

        var tierPrice = AbstractTypeFactory<TierPrice>.TryCreateInstance();
        tierPrice.Price = item.SalePrice;
        tierPrice.Quantity = item.Quantity;

        result.ProposalPrices = new[] { tierPrice };

        return result;
    }

    protected virtual IList<QuoteAddress> FromCartAddresses(ShoppingCart cart)
    {
        return (cart.Addresses ?? Array.Empty<CartAddress>())
            .Concat(cart.Shipments?.Select(x => x.DeliveryAddress) ?? Array.Empty<CartAddress>())
            .Concat(cart.Payments?.Select(x => x.BillingAddress) ?? Array.Empty<CartAddress>())
            .Where(x => x != null)
            .Select(FromCartAddress)
            .Distinct()
            .ToList();
    }

    protected virtual QuoteAddress FromCartAddress(CartAddress address)
    {
        var result = AbstractTypeFactory<QuoteAddress>.TryCreateInstance();

        result.Name = address.Name;
        result.OuterId = address.OuterId;
        result.AddressType = address.AddressType;
        result.CountryCode = address.CountryCode;
        result.CountryName = address.CountryName;
        result.PostalCode = address.PostalCode;
        result.RegionId = address.RegionId;
        result.RegionName = address.RegionName;
        result.City = address.City;
        result.Line1 = address.Line1;
        result.Line2 = address.Line2;
        result.Email = address.Email;
        result.Phone = address.Phone;
        result.FirstName = address.FirstName;
        result.LastName = address.LastName;
        result.Organization = address.Organization;

        return result;
    }

    protected virtual ICollection<TaxDetail> FromCartTaxDetails(ICollection<TaxDetail> details)
    {
        return details;
    }

    protected virtual IList<DynamicObjectProperty> FromCartDynamicProperties(ICollection<DynamicObjectProperty> properties)
    {
        return properties
            .Select(FromCartDynamicProperty)
            .ToList();
    }

    protected virtual DynamicObjectProperty FromCartDynamicProperty(DynamicObjectProperty property)
    {
        var result = AbstractTypeFactory<DynamicObjectProperty>.TryCreateInstance();

        result.Name = property.Name;
        result.IsDictionary = property.IsDictionary;
        result.ValueType = property.ValueType;
        result.Values = property.Values;

        return result;
    }


    protected virtual IList<CartLineItem> ToCartItems(ICollection<QuoteItem> items)
    {
        return items
            .Select(ToCartItem)
            .ToList();
    }

    protected virtual CartLineItem ToCartItem(QuoteItem item)
    {
        var result = AbstractTypeFactory<CartLineItem>.TryCreateInstance();

        result.CatalogId = item.CatalogId;
        result.CategoryId = item.CategoryId;
        result.Currency = item.Currency;
        result.ImageUrl = item.ImageUrl;
        result.ListPrice = item.ListPrice;
        result.Name = item.Name;
        result.Note = item.Comment;
        result.ProductId = item.ProductId;
        result.SalePrice = item.SalePrice;
        result.Sku = item.Sku;
        result.TaxType = item.TaxType;

        if (item.SelectedTierPrice != null)
        {
            result.SalePrice = item.SelectedTierPrice.Price;
            result.Quantity = (int)item.SelectedTierPrice.Quantity;
        }

        if (result.ListPrice < result.SalePrice)
        {
            result.ListPrice = result.SalePrice;
        }

        result.DiscountAmount = result.ListPrice - result.SalePrice;
        result.IsReadOnly = true;

        // Workaround for the cart to order converter
        result.Id = Guid.NewGuid().ToString();

        return result;
    }

    protected virtual IList<CartAddress> ToCartAddresses(ICollection<QuoteAddress> addresses)
    {
        return addresses
            .Select(ToCartAddress)
            .Distinct()
            .ToList();
    }

    protected virtual CartAddress ToCartAddress(QuoteAddress address)
    {
        var result = AbstractTypeFactory<CartAddress>.TryCreateInstance();

        result.Name = address.Name;
        result.OuterId = address.OuterId;
        result.AddressType = address.AddressType;
        result.CountryCode = address.CountryCode;
        result.CountryName = address.CountryName;
        result.PostalCode = address.PostalCode;
        result.RegionId = address.RegionId;
        result.RegionName = address.RegionName;
        result.City = address.City;
        result.Line1 = address.Line1;
        result.Line2 = address.Line2;
        result.Email = address.Email;
        result.Phone = address.Phone;
        result.FirstName = address.FirstName;
        result.LastName = address.LastName;
        result.Organization = address.Organization;

        return result;
    }

    protected virtual ICollection<TaxDetail> ToCartTaxDetails(ICollection<TaxDetail> details)
    {
        return details;
    }

    protected virtual IList<DynamicObjectProperty> ToCartDynamicProperties(ICollection<DynamicObjectProperty> properties)
    {
        return properties
            .Select(ToCartDynamicProperty)
            .ToList();
    }

    protected virtual DynamicObjectProperty ToCartDynamicProperty(DynamicObjectProperty property)
    {
        var result = AbstractTypeFactory<DynamicObjectProperty>.TryCreateInstance();

        result.Name = property.Name;
        result.IsDictionary = property.IsDictionary;
        result.ValueType = property.ValueType;
        result.Values = property.Values;

        return result;
    }

    protected virtual IList<Shipment> ToCartShipments(QuoteShipmentMethod shipmentMethod, ICollection<CartAddress> addresses)
    {
        var shipment = AbstractTypeFactory<Shipment>.TryCreateInstance();

        shipment.Currency = shipmentMethod.Currency;
        shipment.ShipmentMethodCode = shipmentMethod.ShipmentMethodCode;
        shipment.ShipmentMethodOption = shipmentMethod.OptionName;
        shipment.DeliveryAddress = addresses.FirstOrDefault(x => x.AddressType == AddressType.Shipping);

        return new List<Shipment> { shipment };
    }

    protected virtual IList<Payment> ToCartPayments(QuoteRequest quote, ICollection<CartAddress> addresses)
    {
        var payment = AbstractTypeFactory<Payment>.TryCreateInstance();

        payment.Currency = quote.Currency;
        payment.Amount = quote.Totals?.GrandTotalInclTax ?? 0m;
        payment.BillingAddress = addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing);

        return new List<Payment> { payment };
    }
}