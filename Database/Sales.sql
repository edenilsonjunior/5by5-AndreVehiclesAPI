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

