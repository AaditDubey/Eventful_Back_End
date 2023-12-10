using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Time.Commerce.Domains.Entities.Base;

namespace Time.Commerce.Domains.Entities.Sales
{
    public class OrderTax : SubBaseEntity
    {
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Percent { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Amount { get; set; }
    }
}
