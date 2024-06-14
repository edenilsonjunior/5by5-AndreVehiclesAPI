using Microsoft.AspNetCore.Mvc;
using Models.Financials;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Services.Financials;
using System.Text;

namespace AndreVehicles.BankAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BanksController : ControllerBase
{

    private readonly ConnectionFactory _factory;
    const string QUEUE_NAME = "BankQueue";

    public BanksController(ConnectionFactory factory)
    {
        _factory = factory;
    }

    /*
        [HttpGet]
        public ActionResult<List<Bank>> Get()
        {
            var bankList = _bankService.Get();
            if (bankList == null)
                return NotFound();


            return bankList;
        }

        [HttpGet("{cnpj}", Name = "GetBank")]
        public ActionResult<Bank> Get(string cnpj)
        {
            var bank = _bankService.Get(cnpj);
            if (bank == null)
                return NotFound();

            return bank;
        }*/

    [HttpPost]
    public ActionResult<Bank> Post(Bank bank)
    {
        // acting as a producer
        using (var connection = _factory.CreateConnection())
        {
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: QUEUE_NAME,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var stringFieldBank = JsonConvert.SerializeObject(bank);
            var bytesBank = Encoding.UTF8.GetBytes(stringFieldBank);

            channel.BasicPublish(
                exchange: "",
                routingKey: QUEUE_NAME,
                basicProperties: null,
                body: bytesBank
                );
        }
        return Accepted();
    }

    /*
        [HttpGet("/GetBankByCnpj/{cnpj}")]
        public ActionResult<Bank> GetBankByCnpj(string cnpj)
        {
            var bank = _bankService.Get(cnpj);

            return bank != null ? Ok(bank) : NotFound();
        }*/ 

}
