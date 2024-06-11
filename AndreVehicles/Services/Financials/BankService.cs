using Models.Financials;
using MongoDB.Driver;
using Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Financials;

public class BankService
{
    private readonly IMongoCollection<Bank> _bank;

    public BankService(IMongoDataBaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _bank = database.GetCollection<Bank>(settings.BankCollectionName);
    }

    public List<Bank> Get() => _bank.Find(address => true).ToList();

    public Bank Get(string cnpj) => _bank.Find(bank => bank.Cnpj == cnpj).FirstOrDefault();

    public Bank Create(Bank termsOfUse)
    {
        try
        {
            _bank.InsertOne(termsOfUse);
            return termsOfUse;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
