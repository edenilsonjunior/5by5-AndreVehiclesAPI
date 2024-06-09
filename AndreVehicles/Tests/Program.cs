using Services.Cars;
using Services.People;
using Services.Sales;
using Models.Cars;
using Models.People;
using Models.Sales;

var carService = new CarService();
var operationService = new OperationService();
var carOperationService = new CarOperationService();
var purchaseService = new PurchaseService();

var addressService = new AddressService();
var customerService = new CustomerService();
var employeeService = new EmployeeService();

var paymentService = new PaymentService();
var saleService = new SaleService();

string dapper = "dapper";
string ado = "ado";

// mock for Models.Cars
Car c1 = new("AAA-0000", "carro", 2000, 2000, "vermelho", sold: false);
Car c2 = new("AAA-0001", "carro", 2000, 2000, "branco", sold: false);

Operation o1 = new("lavar carro");
Operation o2 = new("trocar pneu");

CarOperation co1 = new(c1, o1, true);
CarOperation co2 = new(c1, o2, true);

Purchase p1 = new(c1, 30000, DateTime.Now);
Purchase p2 = new(c2, 50000, DateTime.Now);

// mock for Models.People

var address1 = new Address("Rua das Flores", "12345-678", "Centro", "Rua", "Apto 101", 100, "SP", "São Paulo");
var address2 = new Address("Avenida Brasil", "87654-321", "Jardim América", "Avenida", "Casa 2", 200, "RJ", "Rio de Janeiro");


var customer1 = new Customer("123.456.789-00", "João da Silva", new DateTime(1985, 5, 20), address1, "(11) 98765-4321", "joao.silva@example.com", 5000.00m);
var customer2 = new Customer("987.654.321-00", "Maria Oliveira", new DateTime(1990, 10, 15), address2, "(21) 99876-5432", "maria.oliveira@example.com", 7500.00m);


var role1 = new Role("Vendedor");
var role2 = new Role("Gerente");


var employee1 = new Employee("111.222.333-44", "Carlos Almeida", new DateTime(1980, 3, 10), address1, "(11) 91234-5678", "carlos.almeida@example.com", role1, 1000.00m, 0.05m);
var employee2 = new Employee("555.666.777-88", "Fernanda Costa", new DateTime(1975, 7, 25), address2, "(21) 92345-6789", "fernanda.costa@example.com", role2, 2000.00m, 0.10m);




// Mocks for Models.Sales

var bankSlip1 = new BankSlip(1, 123456, new DateTime(2024, 7, 10));
var bankSlip2 = new BankSlip(2, 654321, new DateTime(2024, 8, 15));


var card1 = new Card("4111 1111 1111 1111", "123", "12/25", "João da Silva");
var card2 = new Card("4222 2222 2222 2222", "456", "01/26", "Maria Oliveira");


var pixType1 = new PixType("CPF");
var pixType2 = new PixType("Email");


var pix1 = new Pix(pixType1, "123.456.789-00");
var pix2 = new Pix(pixType2, "maria.oliveira@example.com");


var payment1 = new Payment(1, card1, bankSlip1, pix1, new DateTime(2024, 6, 5));
var payment2 = new Payment(2, card2, bankSlip2, pix2, new DateTime(2024, 6, 7));


var sale1 = new Sale(1, customer1, employee1, c1, payment1, new DateTime(2024, 6, 8), 50000.00m);
var sale2 = new Sale(2, customer2, employee2, c2, payment2, new DateTime(2024, 6, 9), 70000.00m);


#region "Post tests

// Services.Cars
carService.Post(dapper, c1);
carService.Post(ado, c2);

operationService.Post(dapper, o1);
operationService.Post(ado, o2);

carOperationService.Post(dapper, co1);
carOperationService.Post(ado, co2);

purchaseService.Post(dapper, p1);
purchaseService.Post(ado, p2);


// Services.People
addressService.Post(dapper, address1);
addressService.Post(ado, address2);

customerService.Post(dapper, customer1);
customerService.Post(ado, customer2);

employeeService.Post(dapper, employee1);
employeeService.Post(ado, employee2);

// Services.Sales
paymentService.Post(dapper, payment1);
paymentService.Post(ado, payment2);

saleService.Post(dapper, sale1);
saleService.Post(ado, sale2);

#endregion






#region "GetAll tests"
var carsDapper = carService.Get(dapper);
Console.WriteLine("GetAll Cars DAPPER:");
foreach (var item in carsDapper)
    Console.WriteLine(item);


var carsAdo = carService.Get(ado);
Console.WriteLine("GetAll Cars ADO.NET:");
foreach (var item in carsAdo)
    Console.WriteLine(item);


var operationsDapper = operationService.Get(dapper);
Console.WriteLine("GetAll Operations DAPPER:");
foreach (var item in operationsDapper)
    Console.WriteLine(item);


var operationsAdo = operationService.Get(ado);
Console.WriteLine("GetAll Operations ADO.NET:");
foreach (var item in operationsAdo)
    Console.WriteLine(item);


var carOperationsDapper = carOperationService.Get(dapper);
Console.WriteLine("GetAll CarOperations DAPPER:");
foreach (var item in carOperationsDapper)
    Console.WriteLine(item);

var carOperationsAdo = carOperationService.Get(ado);
Console.WriteLine("GetAll CarOperations ADO.NET:");
foreach (var item in carOperationsAdo)
    Console.WriteLine(item);

var purchasesDapper = purchaseService.Get(dapper);
Console.WriteLine("GetAll Purchases DAPPER:");
foreach (var item in purchasesDapper)
    Console.WriteLine(item);

var purchasesAdo = purchaseService.Get(ado);
Console.WriteLine("GetAll Purchases ADO.NET:");
foreach (var item in purchasesAdo)
    Console.WriteLine(item);

var addressesDapper = addressService.Get(dapper);
Console.WriteLine("GetAll Addresses DAPPER:");
foreach (var item in addressesDapper)
    Console.WriteLine(item);


var addressesAdo = addressService.Get(ado);
Console.WriteLine("GetAll Addresses ADO.NET:");
foreach (var item in addressesAdo)
    Console.WriteLine(item);



var customersDapper = customerService.Get(dapper);
Console.WriteLine("GetAll Customers DAPPER:");
foreach (var item in customersDapper)
    Console.WriteLine(item);


var customersAdo = customerService.Get(ado);
Console.WriteLine("GetAll Customers ADO.NET:");
foreach (var item in customersAdo)
    Console.WriteLine(item);



var employeesDapper = employeeService.Get(dapper);
Console.WriteLine("GetAll Employees DAPPER:");
foreach (var item in employeesDapper)
    Console.WriteLine(item);

var employeesAdo = employeeService.Get(ado);
Console.WriteLine("GetAll Employees ADO.NET:");
foreach (var item in employeesAdo)
    Console.WriteLine(item);




var paymentsDapper = paymentService.Get(dapper);
Console.WriteLine("GetAll payments DAPPER:");
foreach (var item in paymentsDapper)
    Console.WriteLine(item);

var paymentsAdo = paymentService.Get(ado);
Console.WriteLine("GetAll payments ADO.NET:");
foreach (var item in paymentsAdo)
    Console.WriteLine(item);



var salesDapper = saleService.Get(dapper);
Console.WriteLine("GetAll Sales DAPPER:");
foreach (var item in salesDapper)
    Console.WriteLine(item);

var salesAdo = saleService.Get(ado);
Console.WriteLine("GetAll Sales ADO.NET:");
foreach (var item in salesAdo)
    Console.WriteLine(item);



#endregion

#region "GetByPK tests"




#endregion
