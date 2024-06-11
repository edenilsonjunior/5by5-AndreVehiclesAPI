IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'DBAndreVehicles')
    CREATE DATABASE DBAndreVehicles;

use DBAndreVehicles;

if exists (select * from sys.tables where name = 'Purchase')
    DROP TABLE Purchase;

if exists (select * from sys.tables where name = 'CarOperation')
    DROP TABLE CarOperation;

if exists (select * from sys.tables where name = 'Car')
    DROP TABLE Car;

if exists (select * from sys.tables where name = 'Operation')
    DROP TABLE Operation;

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
