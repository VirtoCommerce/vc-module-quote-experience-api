using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteExperienceApi.Data.Services;

public interface IQuoteConverter
{
    QuoteRequest ConvertFromCart(ShoppingCart cart);
    ShoppingCart ConvertToCart(QuoteRequest quote);
}
