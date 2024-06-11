namespace Services.Utils;

public interface IMongoDataBaseSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
 
    
    string TermsOfUseCollectionName { get; set; }
    string TermsOfUseAcceptCollectionName { get; set; }
    string BankCollectionName { get; set; }
}
