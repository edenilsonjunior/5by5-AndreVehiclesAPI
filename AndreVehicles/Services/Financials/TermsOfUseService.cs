using Models.Financials;
using MongoDB.Driver;
using Services.Utils;

namespace Services.Financials;

public class TermsOfUseService
{
    private readonly IMongoCollection<TermsOfUse> _termsOfUse;

    public TermsOfUseService(IMongoDataBaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _termsOfUse = database.GetCollection<TermsOfUse>(settings.TermsOfUseCollectionName);
    }

    public List<TermsOfUse> Get() => _termsOfUse.Find(address => true).ToList();

    public TermsOfUse Get(string id) => _termsOfUse.Find(termsOfUse => termsOfUse.Id == id).FirstOrDefault();

    public TermsOfUse Create(TermsOfUse termsOfUse)
    {
        _termsOfUse.InsertOne(termsOfUse);
        return termsOfUse;
    }
}
