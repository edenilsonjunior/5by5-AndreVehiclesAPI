using Models.People;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Models.Financials;

public class TermsOfUseAccept
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public Customer Customer { get; set; }
    public TermsOfUse TermsOfUse { get; set; }
    public DateTime AcceptDate { get; set; }
}
