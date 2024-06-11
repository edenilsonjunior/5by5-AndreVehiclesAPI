using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Models.Financials;

public class Bank
{
    [BsonId]
    public string Cnpj { get; set; }
    public string Name { get; set; }
    public DateTime FoundationDate { get; set; }
}
