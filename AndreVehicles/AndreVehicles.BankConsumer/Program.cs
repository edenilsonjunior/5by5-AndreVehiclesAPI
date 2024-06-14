using Models.Financials;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AndreVehicles.BankConsumer;

internal class Program
{
    static void Main(string[] args)
    {
        const string QUEUE_NAME = "BankQueue";

        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QUEUE_NAME,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

                var bankService = new BankService();

                while (true)
                {
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var returnMessage = Encoding.UTF8.GetString(body);
                        var bank = JsonConvert.DeserializeObject<Bank>(returnMessage);


                        string uriMongoApi = "";
                        string uriSqlApi = "";

                        Task<Bank> taskMongo = bankService.Post(uriMongoApi, bank);
                        Task<Bank> taskSQL = bankService.Post(uriSqlApi,bank);

                        Task.WaitAll(taskMongo, taskSQL);
                        
                        var bankMongo = taskMongo.Result;
                        var bankSQL = taskSQL.Result;

                        if(bankMongo != null)
                            Console.WriteLine($"Bank {bank.Name} inserted successfully in MongoDB!");
                        else
                            Console.WriteLine($"Failed to insert Bank {bank.Name} in MongoDB!");

                        if (bankSQL != null)
                            Console.WriteLine($"Bank {bank.Name} inserted successfully in SQL!");
                        else
                            Console.WriteLine($"Failed to insert Bank {bank.Name} in SQL!");
                    };

                    channel.BasicConsume(queue: QUEUE_NAME,
                                         autoAck: true,
                                         consumer: consumer);

                    Thread.Sleep(2000);
                }
            }
        }
    }
}
