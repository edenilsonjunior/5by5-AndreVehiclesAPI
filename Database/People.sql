IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'DBAndreVehicles')
    CREATE DATABASE DBAndreVehicles;

USE DBAndreVehicles;

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


CREATE TABLE Address(

    Id INT IDENTITY(1,1) NOT NULL,
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
    AddressId INT NOT NULL,
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
