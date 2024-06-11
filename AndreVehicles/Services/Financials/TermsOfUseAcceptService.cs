using Models.Financials;
using MongoDB.Driver;
using Services.Utils;

namespace Services.Financials;

public class TermsOfUseAcceptService
{
    private readonly IMongoCollection<TermsOfUseAccept> _termsOfUseAccept;

    public TermsOfUseAcceptService(IMongoDataBaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _termsOfUseAccept = database.GetCollection<TermsOfUseAccept>(settings.TermsOfUseAcceptCollectionName);
    }

    public List<TermsOfUseAccept> Get() => _termsOfUseAccept.Find(address => true).ToList();

    public TermsOfUseAccept Get(string id) => _termsOfUseAccept.Find(termsOfUseAccept => termsOfUseAccept.Id == id).FirstOrDefault();

    public TermsOfUseAccept Create(TermsOfUseAccept termsOfUseAccept)
    {
        _termsOfUseAccept.InsertOne(termsOfUseAccept);
        return termsOfUseAccept;
    }
}
