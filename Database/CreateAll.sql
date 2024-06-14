IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'DBAndreVehicles')
    CREATE DATABASE DBAndreVehicles;

USE DBAndreVehicles;

if exists (select * from sys.tables where name = 'Sale')
    DROP TABLE Sale;

if exists (select * from sys.tables where name = 'Payment')
    DROP TABLE Payment;

if exists (select * from sys.tables where name = 'Pix')
    DROP TABLE Pix;

if exists (select * from sys.tables where name = 'PixType')
    DROP TABLE PixType;

if exists (select * from sys.tables where name = 'Card')
    DROP TABLE Card;

if exists (select * from sys.tables where name = 'BankSlip')
    DROP TABLE BankSlip;

if exists(select * from sys.tables where name = 'Dependent')
    DROP TABLE Dependent;

if exists(select * from sys.tables where name = 'Customer')
    DROP TABLE Customer;

if exists(select * from sys.tables where name = 'Employee')
    DROP TABLE Employee;

if exists(select * from sys.tables where name = 'Role') 
    DROP TABLE Role;

if exists(select * from sys.tables where name = 'Person')
    DROP TABLE Person;

if exists(select * from sys.tables where name = 'Address')
    DROP TABLE Address;

if exists (select * from sys.tables where name = 'Purchase')
    DROP TABLE Purchase;

if exists (select * from sys.tables where name = 'CarOperation')
    DROP TABLE CarOperation;

if exists (select * from sys.tables where name = 'Car')
    DROP TABLE Car;

if exists (select * from sys.tables where name = 'Operation')
    DROP TABLE Operation;




-- Creating tables

CREATE TABLE Car(

    Plate NVARCHAR(8) NOT NULL,
    Name NVARCHAR(50) NOT NULL,
    YearManufacture INT NOT NULL,
    YearModel INT NOT NULL,
    Color NVARCHAR(50) NOT NULL,
    Sold BIT NOT NULL

    CONSTRAINT pk_car PRIMARY KEY (Plate)
);


CREATE TABLE Operation(

    Id INT IDENTITY(1,1) NOT NULL,
    Description NVARCHAR(50) NOT NULL

    CONSTRAINT pk_operation PRIMARY KEY (Id)
);

CREATE TABLE CarOperation(

    Id INT IDENTITY(1,1),
    CarPlate NVARCHAR(8) NOT NULL,
    OperationId INT NOT NULL,
    Status BIT NOT NULL

    CONSTRAINT pk_car_operation PRIMARY KEY (Id),
    CONSTRAINT fk_car_operation_car FOREIGN KEY (CarPlate) REFERENCES Car(Plate),
    CONSTRAINT fk_car_operation_operation FOREIGN KEY (OperationId) REFERENCES Operation(Id)
);


CREATE TABLE Purchase(

    Id INT IDENTITY(1,1) NOT NULL,
    CarPlate NVARCHAR(8) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    PurchaseDate DATETIME NOT NULL

    CONSTRAINT pk_purchase PRIMARY KEY (Id),
    CONSTRAINT fk_purchase_car FOREIGN KEY (CarPlate) REFERENCES Car(Plate)
);



CREATE TABLE Address(

    Id varchar(255) NOT NULL,
    Street NVARCHAR(50) NOT NULL,
    PostalCode NVARCHAR(8) NOT NULL,
    District NVARCHAR(50) NOT NULL,
    StreetType NVARCHAR(50) NOT NULL,
    AdditionalInfo NVARCHAR(50) NOT NULL,
    Number INT NOT NULL,
    State NVARCHAR(50) NOT NULL,
    City NVARCHAR(50) NOT NULL

    CONSTRAINT pk_address PRIMARY KEY (Id)
);


CREATE TABLE Person(

    Document NVARCHAR(14) NOT NULL,
    Name NVARCHAR(50) NOT NULL,
    BirthDate DATETIME NOT NULL,
    AddressId varchar(255) NOT NULL,
    Phone NVARCHAR(15) NOT NULL,
    Email NVARCHAR(50) NOT NULL

    CONSTRAINT pk_person PRIMARY KEY (Document),
    CONSTRAINT fk_person_address FOREIGN KEY (AddressId) REFERENCES Address(Id)
);


CREATE TABLE Role(

    Id INT IDENTITY(1,1) NOT NULL,
    Description NVARCHAR(50) NOT NULL

    CONSTRAINT pk_role PRIMARY KEY (Id)
);


CREATE TABLE Employee(

    Document NVARCHAR(14) NOT NULL,
    RoleId INT NOT NULL,
    CommissionValue DECIMAL(13,2) NOT NULL,
    Commission DECIMAL(13,2) NOT NULL

    CONSTRAINT pk_employee PRIMARY KEY (Document),
    CONSTRAINT fk_employee_person FOREIGN KEY (Document) REFERENCES Person(Document),
    CONSTRAINT fk_employee_role FOREIGN KEY (RoleId) REFERENCES Role(Id)
);


CREATE TABLE Customer(

    Document NVARCHAR(14) NOT NULL,
    Income DECIMAL(13,2) NOT NULL

    CONSTRAINT pk_customer PRIMARY KEY (Document),
    CONSTRAINT fk_customer_person FOREIGN KEY (Document) REFERENCES Person(Document)
);

CREATE TABLE Dependent
(
    CustomerDocument NVARCHAR(14) NOT NULL,
    Document NVARCHAR(14) NOT NULL,

    CONSTRAINT pk_dependent PRIMARY KEY (Document),
    CONSTRAINT fk_dependent_person FOREIGN KEY (Document) REFERENCES Person(Document)
);

CREATE TABLE BankSlip(

    Id INT IDENTITY(1,1) NOT NULL,
    Number INT NOT NULL,
    DueDate DATETIME NOT NULL

    CONSTRAINT pk_bank_slip PRIMARY KEY (Id)
);


CREATE TABLE Card(

    CardNumber NVARCHAR(16) NOT NULL,
    SecurityCode NVARCHAR(3) NOT NULL,
    ExpirationDate NVARCHAR(5) NOT NULL,
    CardHolderName NVARCHAR(50) NOT NULL

    CONSTRAINT pk_card PRIMARY KEY (CardNumber)
);


CREATE TABLE PixType(

    Id INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(50) NOT NULL

    CONSTRAINT pk_pix_type PRIMARY KEY (Id)
);


CREATE TABLE Pix(

    Id INT IDENTITY(1,1) NOT NULL,
    Type INT NOT NULL,
    PixKey NVARCHAR(50) NOT NULL

    CONSTRAINT pk_pix PRIMARY KEY (Id),
    CONSTRAINT fk_pix_pix_type FOREIGN KEY (Type) REFERENCES PixType(Id)
);


CREATE TABLE Payment(

    Id INT IDENTITY(1,1) NOT NULL,
    CardNumber NVARCHAR(16),
    BankSlipId INT,
    PixId INT,
    PaymentDate DATETIME NOT NULL

    CONSTRAINT pk_payment PRIMARY KEY (Id),
    CONSTRAINT fk_payment_card FOREIGN KEY (CardNumber) REFERENCES Card(CardNumber),
    CONSTRAINT fk_payment_bank_slip FOREIGN KEY (BankSlipId) REFERENCES BankSlip(Id),
    CONSTRAINT fk_payment_pix FOREIGN KEY (PixId) REFERENCES Pix(Id)
);


CREATE TABLE Sale(

    Id INT IDENTITY(1,1) NOT NULL,
    CustomerDocument NVARCHAR(14) NOT NULL,
    EmployeeDocument NVARCHAR(14) NOT NULL,
    CarPlate NVARCHAR(8) NOT NULL,
    PaymentId INT NOT NULL,
    SaleDate DATETIME NOT NULL,
    SalePrice DECIMAL(18,2) NOT NULL

    CONSTRAINT pk_sale PRIMARY KEY (Id),
    CONSTRAINT fk_sale_customer FOREIGN KEY (CustomerDocument) REFERENCES Customer(Document),
    CONSTRAINT fk_sale_employee FOREIGN KEY (EmployeeDocument) REFERENCES Employee(Document),
    CONSTRAINT fk_sale_car FOREIGN KEY (CarPlate) REFERENCES Car(Plate),
    CONSTRAINT fk_sale_payment FOREIGN KEY (PaymentId) REFERENCES Payment(Id)
);

CREATE TABLE Bank(

    Cnpj VARCHAR(255) NOT NULL,
    Name VARCHAR(255) NOT NULL,
    FoundationDate DATETIME NOT NULL,

    CONSTRAINT pk_bank PRIMARY KEY (Cnpj)
);


CREATE TABLE CarFinancing(

    Id INT IDENTITY(1,1) NOT NULL,
    SaleId INT NOT NULL,
    FinancingDate DATETIME NOT NULL,
    BankCnpj VARCHAR(255),

    CONSTRAINT pk_carFinancing PRIMARY KEY (Id),
    CONSTRAINT fk_carFinancing_sale FOREIGN KEY (SaleId) REFERENCES Sale(Id)/*,
    CONSTRAINT fk_carFinancing_bank FOREIGN KEY (BankCnpj) REFERENCES Bank(Cnpj),*/
);


CREATE TABLE Bank(

    Cnpj VARCHAR(255) NOT NULL,
    Name VARCHAR(255) NOT NULL,
    FoundationDate DATETIME NOT NULL,

    CONSTRAINT pk_bank PRIMARY KEY (Cnpj)
);


CREATE TABLE CarFinancing(

    Id INT IDENTITY(1,1) NOT NULL,
    SaleId INT NOT NULL,
    FinancingDate DATETIME NOT NULL,
    BankCnpj VARCHAR(255),

    CONSTRAINT pk_carFinancing PRIMARY KEY (Id),
    CONSTRAINT fk_carFinancing_sale FOREIGN KEY (SaleId) REFERENCES Sale(Id)/*,
    CONSTRAINT fk_carFinancing_bank FOREIGN KEY (BankCnpj) REFERENCES Bank(Cnpj),*/
);


CREATE TABLE Insurance(

    Id INT IDENTITY(1,1) NOT NULL,
    CustomerDocument NVARCHAR(14) NOT NULL,
    Deductible NVARCHAR(255) NOT NULL,
    CarPlate NVARCHAR(8) NOT NULL,
    MainDriverDocument NVARCHAR(14) NOT NULL

    CONSTRAINT pk_insurance PRIMARY KEY (Id),
    CONSTRAINT fk_insurance_customer FOREIGN KEY (CustomerDocument) REFERENCES Customer(Document),
    CONSTRAINT fk_insurance_car FOREIGN KEY (CarPlate) REFERENCES Car(Plate),
    CONSTRAINT fk_insurance_main_driver FOREIGN KEY (MainDriverDocument) REFERENCES Person(Document)
);