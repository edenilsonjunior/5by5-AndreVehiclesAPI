using Microsoft.Data.SqlClient;
using Models.Sales;
using System.Data;

namespace Repositories.Sales;

public class PaymentRepository
{
    private readonly string _connectionString;
    private readonly AdoUtilsRepository _adoUtils;

    public PaymentRepository()
    {
        _connectionString = "Data Source=127.0.0.1; Initial Catalog=DBAndreVehicles; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";
        _adoUtils = new AdoUtilsRepository();
    }


    // Get all and get by id
    public List<Payment>? Get(string technology)
    {
        if (technology.Equals("dapper"))
        {
            var list = new List<Payment>();

            foreach (var row in DapperUtilsRepository<dynamic>.GetAll(Payment.GETALL))
            {
                Card? card = null;
                Pix? pix = null;
                BankSlip? bankSlip = null;

                if (row.CardNumber != null)
                    card = new(row.CardNumber, row.SecurityCode, row.ExpirationDate, row.CardHolderName);

                if (row.BankSlipId != null)
                    bankSlip = new(row.BankSlipId, row.BankSlipNumber, row.BankSlipDueDate);

                if (row.PixId != null)
                    pix = RetrievePix(row);

                var payment = new Payment(row.Id, card, bankSlip, pix, row.PaymentDate);
                list.Add(payment);
            }
            return list;
        }
        else if (technology.Equals("ado"))
        {
            DataTable dataTable = _adoUtils.ExecuteReader(Payment.GETALL, new()).Result;

            if (dataTable.Rows.Count == 0) return null;

            var list = new List<Payment>();

            foreach (DataRow row in dataTable.Rows)
            {
                var card = RetrieveCard(row);
                var pix = RetrievePix(row);
                var bankSlip = RetrieveBankSlip(row);

                Payment payment = new(Convert.ToInt32(row["Id"]), card, bankSlip, pix, Convert.ToDateTime(row["PaymentDate"]));
                list.Add(payment);
            }
            return list;
        }
        return null;
    }

    public Payment Get(string technology, int id)
    {
        if (technology.Equals("dapper"))
        {
            dynamic data = DapperUtilsRepository<Payment>.Get(Payment.GET, new { Id = id });

            if (data == null) return null;

            Card? card = null;
            Pix? pix = null;
            BankSlip? bankSlip = null;

            if (data.CardNumber != null)
                card = new(data.CardNumber, data.SecurityCode, data.ExpirationDate, data.CardHolderName);

            if (data.BankSlipId != null)
                bankSlip = new(data.BankSlipId, data.BankSlipNumber, data.BankSlipDueDate);

            if (data.PixId != null)
                pix = RetrievePix(data);

            return new Payment(data.Id, card, bankSlip, pix, data.PaymentDate);
        }

        if (technology.Equals("ado"))
        {
            DataTable dataTable = _adoUtils.ExecuteReader(Payment.GET, new() { new("Id", id) }).Result;

            if (dataTable.Rows.Count == 0)
                return null;

            DataRow row = dataTable.Rows[0];

            var card = RetrieveCard(row);
            var pix = RetrievePix(row);
            var bankSlip = RetrieveBankSlip(row);

            return new Payment(Convert.ToInt32(row["Id"]), card, bankSlip, pix, Convert.ToDateTime(row["PaymentDate"]));
        }

        return null;
    }



    // Post methods
    public bool Post(string technology, Payment payment)
    {
        return technology switch
        {
            "dapper" => InsertPaymentDapper(payment),
            "ado" => InsertPaymentAdo(payment),
            _ => false,
        };
    }


    // Post aux methods
    private bool InsertPaymentDapper(Payment payment)
    {
        if (payment.Pix != null)
        {
            payment.Pix.Type.Id = DapperUtilsRepository<PixType>.InsertWithScalar(PixType.INSERT, new { payment.Pix.Type.Name });


            object pix = new
            {
                Type = payment.Pix.Type.Id,
                PixKey = payment.Pix.PixKey
            };

            payment.Pix.Id = DapperUtilsRepository<Pix>.InsertWithScalar(Pix.POST, pix);
        }
        else if (payment.BankSlip != null)
        {
            payment.BankSlip.Id = DapperUtilsRepository<BankSlip>.InsertWithScalar(BankSlip.POST, payment.BankSlip);
        }
        else if (payment.Card != null)
        {
            DapperUtilsRepository<Card>.Insert(Card.POST, payment.Card);
        }

        object obj = new
        {
            CardNumber = payment.Card?.CardNumber,
            BankSlipId = payment.BankSlip?.Id,
            PixId = payment.Pix?.Id,
            PaymentDate = payment.PaymentDate
        };

        return DapperUtilsRepository<dynamic>.Insert(Payment.Post, obj);
    }

    private bool InsertPaymentAdo(Payment payment)
    {

        if (payment.Card != null)
        {
            var list = new List<SqlParameter>();
            list.Add(new("@CardNumber", payment.Card.CardNumber));
            list.Add(new("@SecurityCode", payment.Card.SecurityCode));
            list.Add(new("@ExpirationDate", payment.Card.ExpirationDate));
            list.Add(new("@CardHolderName", payment.Card.CardHolderName));

            _ = _adoUtils.ExecuteNonQuery(Card.POST, list).Result;
        }

        if (payment.Pix != null)
        {
            var pixTypeId = new List<SqlParameter>() { new("@Name", payment.Pix.Type.Name) };
            payment.Pix.Type.Id = _adoUtils.ExecuteScalar(PixType.INSERT, pixTypeId).Result;

            var pixParameters = new List<SqlParameter>()
            {
                new("@Type", payment.Pix.Type.Id),
                new("@PixKey", payment.Pix.PixKey)
            };

            payment.Pix.Id = _adoUtils.ExecuteScalar(Pix.POST, pixParameters).Result;
        }

        if (payment.BankSlip != null)
        {
            var list = new List<SqlParameter>()
            {
                new("@Number", payment.BankSlip.Number),
                new("@DueDate", payment.BankSlip.DueDate)
            };

            payment.BankSlip.Id = _adoUtils.ExecuteScalar(BankSlip.POST, list).Result;
        }


        var paymentParameters = new List<SqlParameter>()
        {
            new("@CardNumber", payment.Card == null ? DBNull.Value : payment.Card.CardNumber),
            new("@BankSlipId", payment.BankSlip == null ? DBNull.Value :  payment.BankSlip?.Id ),
            new("@PixId", payment.Pix == null ? DBNull.Value : payment.Pix.Id),
            new("@PaymentDate", payment.PaymentDate)
        };

        return _adoUtils.ExecuteNonQuery(Payment.Post, paymentParameters).Result > 0;

    }


    // Get aux methods for
    private Card? RetrieveCard(DataRow row)
    {
        if (row["CardNumber"] == DBNull.Value)
            return null;

        return new()
        {
            CardNumber = row["CardNumber"].ToString(),
            SecurityCode = row["SecurityCode"].ToString(),
            ExpirationDate = row["ExpirationDate"].ToString(),
            CardHolderName = row["CardHolderName"].ToString()
        };
    }

    private Pix? RetrievePix(DataRow row)
    {
        if (row["PixId"] == DBNull.Value)
            return null;

        return new Pix
        {
            Id = Convert.ToInt32(row["PixId"]),
            PixKey = row["PixKey"].ToString(),
            Type = new PixType
            {
                Id = Convert.ToInt32(row["PixTypeId"]),
                Name = row["PixTypeName"].ToString()
            }
        };
    }

    private BankSlip? RetrieveBankSlip(DataRow row)
    {
        if (row["BankSlipId"] == DBNull.Value)
            return null;

        return new BankSlip
        {
            Id = Convert.ToInt32(row["BankSlipId"]),
            Number = Convert.ToInt32(row["BankSlipNumber"]),
            DueDate = Convert.ToDateTime(row["BankSlipDueDate"])
        };
    }

    private Pix RetrievePix(dynamic row)
    {
        return new Pix()
        {
            Id = row.PixId,
            PixKey = row.PixKey,
            Type = new()
            {
                Id = row.PixTypeId,
                Name = row.PixTypeName
            }
        };
    }

}
