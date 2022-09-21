using VirtoCommerce.QuoteModule.Core.Models;

namespace VirtoCommerce.QuoteExperienceApi.Data.Aggregates;

public class QuoteShipmentMethodAggregate
{
    public ShipmentMethod Model { get; set; }
    public QuoteAggregate Quote { get; set; }
}
