using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Models.Financials;

public class TermsOfUse
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Text { get; set; }
    public int Version { get; set; }
    public DateTime RegisterDate { get; set; }
    public bool Status { get; set; }
}
