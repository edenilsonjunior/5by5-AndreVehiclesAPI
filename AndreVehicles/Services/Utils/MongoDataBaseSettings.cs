namespace Services.Utils;

public class MongoDataBaseSettings : IMongoDataBaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string TermsOfUseCollectionName { get; set; }
    public string TermsOfUseAcceptCollectionName { get; set; }
}
