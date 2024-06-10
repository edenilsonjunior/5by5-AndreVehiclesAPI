using Microsoft.Data.SqlClient;
using Models.Sales;
using System.Configuration;

namespace Repositories.Sales;

public class PaymentRepository
{
    private string _connectionString;

    public PaymentRepository()
    {
        _connectionString = "Data Source=127.0.0.1; Initial Catalog=DBAndreVehicles; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";
    }

    public List<Payment> Get(string technology)
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
                {
                    pix = new()
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

                var payment = new Payment(row.Id, card, bankSlip, pix, row.PaymentDate);
                list.Add(payment);
            }
            return list;
        }

        if (technology.Equals("ado"))
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            SqlCommand sqlCommand = new SqlCommand(Payment.GETALL, sqlConnection);

            List<Payment> list = new List<Payment>();

            try
            {
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    Card? card = null;
                    Pix? pix = null;
                    BankSlip? bankSlip = null;

                    if (reader["CardNumber"] != DBNull.Value)
                    {
                        card = new Card()
                        {
                            CardNumber = reader["CardNumber"].ToString(),
                            SecurityCode = reader["SecurityCode"].ToString(),
                            ExpirationDate = reader["ExpirationDate"].ToString(),
                            CardHolderName = reader["CardHolderName"].ToString()
                        };
                    }

                    if (reader["BankSlipId"] != DBNull.Value)
                    {
                        bankSlip = new()
                        {
                            Id = Convert.ToInt32(reader["BankSlipId"]),
                            Number = Convert.ToInt32(reader["BankSlipNumber"]),
                            DueDate = Convert.ToDateTime(reader["BankSlipDueDate"])
                        };
                    }
                    if (reader["PixId"] != DBNull.Value)
                    {
                        pix = new Pix
                        {
                            Id = Convert.ToInt32(reader["PixId"]),
                            PixKey = reader["PixKey"].ToString(),
                            Type = new PixType
                            {
                                Id = Convert.ToInt32(reader["PixTypeId"]),
                                Name = reader["PixTypeName"].ToString()
                            }
                        };
                    }

                    Payment payment = new Payment(Convert.ToInt32(reader["Id"]), card, bankSlip, pix, Convert.ToDateTime(reader["PaymentDate"]));
                    list.Add(payment);
                }
            }
            catch (Exception)
            {
                return null;
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

            if (data == null)
                return null;

            Card? card = null;
            Pix? pix = null;
            BankSlip? bankSlip = null;

            if (data.CardNumber != null)
                card = new(data.CardNumber, data.SecurityCode, data.ExpirationDate, data.CardHolderName);

            if (data.BankSlipId != null)
                bankSlip = new(data.BankSlipId, data.BankSlipNumber, data.BankSlipDueDate);

            if (data.PixId != null)
            {
                pix = new()
                {
                    Id = data.PixId,
                    PixKey = data.PixKey,
                    Type = new()
                    {
                        Id = data.PixTypeId,
                        Name = data.PixTypeName
                    }
                };
            }

            return new Payment(data.Id, card, bankSlip, pix, data.PaymentDate);
        }

        if (technology.Equals("ado"))
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            SqlCommand sqlCommand = new SqlCommand(Payment.GET, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", id);

            var reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                Card? card = null;
                Pix? pix = null;
                BankSlip? bankSlip = null;


                if (reader["CardNumber"] != DBNull.Value)
                {
                    card = new Card()
                    {
                        CardNumber = reader["CardNumber"].ToString(),
                        SecurityCode = reader["SecurityCode"].ToString(),
                        ExpirationDate = reader["ExpirationDate"].ToString(),
                        CardHolderName = reader["CardHolderName"].ToString()
                    };
                }

                if (reader["BankSlipId"] != DBNull.Value)
                {
                    bankSlip = new()
                    {
                        Id = Convert.ToInt32(reader["BankSlipId"]),
                        Number = Convert.ToInt32(reader["BankSlipNumber"]),
                        DueDate = Convert.ToDateTime(reader["BankSlipDueDate"])
                    };
                }
                if (reader["PixId"] != DBNull.Value)
                {
                    pix = new Pix
                    {
                        Id = Convert.ToInt32(reader["PixId"]),
                        PixKey = reader["PixKey"].ToString(),
                        Type = new PixType
                        {
                            Id = Convert.ToInt32(reader["PixTypeId"]),
                            Name = reader["PixTypeName"].ToString()
                        }
                    };
                }

                return new Payment(Convert.ToInt32(reader["Id"]), card, bankSlip, pix, Convert.ToDateTime(reader["PaymentDate"]));

            }
            else
            {
                return null;
            }

        }

        return null;
    }

    public bool Post(string technology, Payment payment)
    {
        object obj;

        if (technology.Equals("dapper"))
        {
            if (payment.Pix != null)
            {
                obj = new
                {
                    PixKey = payment.Pix.PixKey,
                    Type = payment.Pix.Type.Id
                };

                DapperUtilsRepository<Pix>.Insert(Pix.POST, payment.Pix);
            }

            if (payment.BankSlip != null)
            {
                obj = new
                {
                    Number = payment.BankSlip.Number,
                    DueDate = payment.BankSlip.DueDate
                };

                DapperUtilsRepository<BankSlip>.Insert(BankSlip.POST, payment.BankSlip);
            }

            if (payment.Card != null)
            {
                obj = new
                {
                    CardNumber = payment.Card.CardNumber,
                    SecurityCode = payment.Card.SecurityCode,
                    ExpirationDate = payment.Card.ExpirationDate,
                    CardHolderName = payment.Card.CardHolderName
                };

                DapperUtilsRepository<Card>.Insert(Card.POST, payment.Card);
            }

            obj = new
            {
                CardNumber = payment.Card?.CardNumber,
                BankSlipId = payment.BankSlip?.Id,
                PixId = payment.Pix?.Id,
                PaymentDate = payment.PaymentDate
            };

            return DapperUtilsRepository<Payment>.Insert(Payment.POST, payment);
        }

        if (technology.Equals("ado"))
        {

            using SqlConnection sqlConnection = new SqlConnection(_connectionString);

            SqlCommand sqlCommand;

            sqlConnection.Open();
            if (payment.Card != null)
            {
                string insertCard = "INSERT INTO Card(CardNumber, SecurityCode, ExpirationDate, CardHolderName) VALUES(@CardNumber, @SecurityCode, @ExpirationDate, @CardHolderName);";

                sqlCommand = new SqlCommand(insertCard, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@CardNumber", payment.Card.CardNumber);
                sqlCommand.Parameters.AddWithValue("@SecurityCode", payment.Card.SecurityCode);
                sqlCommand.Parameters.AddWithValue("@ExpirationDate", payment.Card.ExpirationDate);
                sqlCommand.Parameters.AddWithValue("@CardHolderName", payment.Card.CardHolderName);
                sqlCommand.ExecuteNonQuery();
            }


            if (payment.Pix != null)
            {
                string insertPixType = "INSERT INTO PixType(Name) VALUES(@Name); SELECT CAST(SCOPE_IDENTITY() AS INT);";
                sqlCommand = new SqlCommand(insertPixType, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Name", payment.Pix.Type.Name);
                payment.Pix.Type.Id = (int)sqlCommand.ExecuteScalar();

                string insertPix = "INSERT INTO Pix(Type, PixKey) VALUES(@Type, @PixKey); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                sqlCommand = new SqlCommand(insertPix, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Type", payment.Pix.Type.Id);
                sqlCommand.Parameters.AddWithValue("@PixKey", payment.Pix.PixKey);
                payment.Pix.Id = (int)sqlCommand.ExecuteScalar();
            }

            if (payment.BankSlip != null)
            {
                string insertBankSlip = "INSERT INTO BankSlip(Number, DueDate) VALUES(@Number, @DueDate); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                sqlCommand = new SqlCommand(insertBankSlip, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Number", payment.BankSlip.Number);
                sqlCommand.Parameters.AddWithValue("@DueDate", payment.BankSlip.DueDate);
                payment.BankSlip.Id = (int)sqlCommand.ExecuteScalar();
            }

            sqlCommand = new SqlCommand(Payment.POST, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@CardNumber", payment.Card?.CardNumber == null ? DBNull.Value : payment.Card.CardNumber);
            sqlCommand.Parameters.AddWithValue("@BankSlipId", payment.BankSlip?.Id == null ? DBNull.Value : payment.BankSlip.Id);
            sqlCommand.Parameters.AddWithValue("@PixId", payment.Pix?.Id == null ? DBNull.Value : payment.Pix.Id);
            sqlCommand.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
            sqlCommand.ExecuteNonQuery();

            return true;
        }
        return false;
    }
}
